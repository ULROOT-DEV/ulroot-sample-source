using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using LocalUdpSample.Client;
using LocalUdpSample.Protocol;

namespace LocalUdpSample
{
    /// <summary>
    /// LifeSmart Smart Station Local UDP(OpenDev) 샘플 애플리케이션 메인 화면입니다.
    ///
    /// [주요 기능]
    /// 1. UDP 브로드캐스트(12345)를 통한 Smart Station 자동 검색 (Discovery)
    /// 2. 서브 디바이스 목록 조회(GET eps), 씬 목록 조회(GET scene), 버전 조회(getver), 재부팅(reboot)
    /// 3. 단일 서브 디바이스 제어(SET ep)
    /// 4. 로컬 이벤트 수신(cfg:notify 등록 + UDP 리스너 + 240초 주기 자동 갱신)
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// notify 등록 유효시간은 300초입니다(매뉴얼 4장).
        /// 네트워크 지연이나 타이머 오차로 인한 만료를 방지하기 위해 240초(4분)마다 미리 갱신합니다.
        /// </summary>
        private const int NotifyRenewalIntervalMs = 240 * 1000;

        /// <summary>로그 창 최대 문자 수 (장시간 이벤트 수신 시 메모리 누적 방지)</summary>
        private const int MaxLogCharacters = 100000;

        private readonly LifeSmartUdpClient _udpClient = new LifeSmartUdpClient();
        private readonly LifeSmartEventReceiver _eventReceiver = new LifeSmartEventReceiver();

        public MainForm()
        {
            InitializeComponent();

            // 요청/응답 디버그 로그를 UI 로그창에 출력하도록 연결
            _udpClient.OnDebugLog = AppendLog;

            // 이벤트 수신기 콜백 연결
            _eventReceiver.EventReceived += EventReceiver_EventReceived;
            _eventReceiver.ErrorOccurred += EventReceiver_ErrorOccurred;

            // 현재 PC의 활성 사설 IPv4 주소를 이벤트 수신 IP 기본값으로 자동 입력
            txtReceiveIp.Text = GetPreferredLocalIPv4();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            notifyTimer.Interval = NotifyRenewalIntervalMs;
            AppendLog("LifeSmart Local UDP 샘플이 준비되었습니다. Station 정보를 입력하거나 [자동 검색]을 눌러 시작하세요.");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 종료 시 타이머와 UDP 소켓 리소스를 반드시 정리합니다.
            StopEventListening(logStopped: false);
            _eventReceiver.Dispose();
            _udpClient.Dispose();
        }

        #region Smart Station 검색 (Discovery)

        private async void btnDiscover_Click(object sender, EventArgs e)
        {
            btnDiscover.Enabled = false;
            try
            {
                AppendLog("로컬 네트워크에서 Smart Station을 검색 중입니다... (UDP 12345 브로드캐스트)");
                IReadOnlyList<DiscoveredStation> stations = await LifeSmartDiscoveryClient.DiscoverStationsAsync(2000);

                if (stations.Count == 0)
                {
                    AppendLog("검색된 Smart Station이 없습니다. PC와 Smart Station이 같은 네트워크(서브넷)에 있는지 확인하세요.");
                    return;
                }

                AppendLog($"Smart Station {stations.Count}대를 발견했습니다:");
                foreach (DiscoveredStation station in stations)
                {
                    AppendLog($"  - {station}");
                }

                // 첫 번째로 발견된 Smart Station의 IP를 자동으로 입력란에 반영
                txtStationIp.Text = stations[0].IpAddress.ToString();
            }
            catch (Exception ex)
            {
                AppendLog($"[오류] Smart Station 검색 실패: {ex.Message}");
            }
            finally
            {
                btnDiscover.Enabled = true;
            }
        }

        #endregion

        #region 조회 / 설정 API 요청 (GET / SET config)

        private async void btnGetDevices_Click(object sender, EventArgs e)
        {
            await ExecuteStationRequestAsync(
                btnGetDevices,
                "서브 디바이스 목록 조회 (GET eps)",
                config => _udpClient.GetDevicesAsync(config));
        }

        private async void btnGetScenes_Click(object sender, EventArgs e)
        {
            await ExecuteStationRequestAsync(
                btnGetScenes,
                "씬 목록 조회 (GET scene)",
                config => _udpClient.GetScenesAsync(config));
        }

        private async void btnGetVersion_Click(object sender, EventArgs e)
        {
            await ExecuteStationRequestAsync(
                btnGetVersion,
                "Smart Station 버전 조회 (SET config:getver)",
                config => _udpClient.GetVersionAsync(config));
        }

        private async void btnReboot_Click(object sender, EventArgs e)
        {
            // 재부팅은 실제 장비 동작에 영향을 주므로 사용자 확인을 거칩니다.
            DialogResult confirm = MessageBox.Show(
                "Smart Station을 재부팅하시겠습니까?\n재부팅 중에는 일시적으로 디바이스 제어가 중단됩니다.",
                "Smart Station 재부팅 확인",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            await ExecuteStationRequestAsync(
                btnReboot,
                "Smart Station 재부팅 (SET config:reboot)",
                config => _udpClient.RebootStationAsync(config));
        }

        #endregion

        #region 단일 서브 디바이스 제어 (SET ep)

        private void btnPresetOn_Click(object sender, EventArgs e)
        {
            // 일반 스위치/소켓 ON 명령: type = 0x81 (129), val = 1 (매뉴얼 6.2 / 6.3장)
            txtControlType.Text = "129";
            txtControlVal.Text = "1";
        }

        private void btnPresetOff_Click(object sender, EventArgs e)
        {
            // 일반 스위치/소켓 OFF 명령: type = 0x80 (128), val = 0 (매뉴얼 6.2 / 6.3장)
            txtControlType.Text = "128";
            txtControlVal.Text = "0";
        }

        private async void btnControlDevice_Click(object sender, EventArgs e)
        {
            string deviceId = txtDeviceMe.Text.Trim();
            string ioIndex = txtDeviceIdx.Text.Trim();

            if (string.IsNullOrEmpty(deviceId) || string.IsNullOrEmpty(ioIndex))
            {
                MessageBox.Show("Device ID(me)와 IO(idx)를 입력하세요.\n예: me=2711, idx=L1", "입력 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!int.TryParse(txtControlType.Text.Trim(), out int controlType) ||
                !int.TryParse(txtControlVal.Text.Trim(), out int controlVal))
            {
                MessageBox.Show("Type과 Val에는 정수를 입력하세요.\n예: ON은 type=129, val=1 / OFF는 type=128, val=0", "입력 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            await ExecuteStationRequestAsync(
                btnControlDevice,
                $"단일 디바이스 제어 (SET ep: me={deviceId}, idx={ioIndex}, type={controlType}, val={controlVal})",
                config => _udpClient.ControlDeviceAsync(config, deviceId, ioIndex, controlType, controlVal));
        }

        #endregion

        #region 로컬 이벤트 수신 (Local Event - cfg:notify)

        private async void btnToggleReceive_Click(object sender, EventArgs e)
        {
            // 이미 수신 중이면 중지 처리
            if (_eventReceiver.IsRunning)
            {
                StopEventListening(logStopped: true);
                return;
            }

            if (!TryBuildStationConfig(out LifeSmartStationConfig config))
            {
                return;
            }

            string receiveIp = txtReceiveIp.Text.Trim();
            if (!IPAddress.TryParse(receiveIp, out _))
            {
                MessageBox.Show("올바른 수신 IP 주소를 입력하세요.", "입력 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!int.TryParse(txtReceivePort.Text.Trim(), out int receivePort) || receivePort < 1 || receivePort > 65535)
            {
                MessageBox.Show("올바른 수신 Port 번호(1~65535)를 입력하세요.", "입력 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            btnToggleReceive.Enabled = false;
            try
            {
                // 1) 먼저 로컬 UDP 이벤트 리스너를 열어 수신 대기 상태로 전환합니다.
                _eventReceiver.Start(receivePort);

                // 2) Smart Station에 이벤트 수신 대상 IP와 Port를 등록(cfg:notify)합니다.
                LifeSmartPacket response = await _udpClient.RegisterNotifyAsync(config, receiveIp, receivePort);
                AppendLog($"[NOTIFY 등록 완료]\r\n{response.ToPrettyJson()}");

                // 3) 300초 만료 방지를 위한 자동 갱신 타이머 시작 (240초 주기)
                notifyTimer.Start();

                SetEventListeningUiState(isListening: true, receiveIp: receiveIp, receivePort: receivePort);
            }
            catch (Exception ex)
            {
                // 등록 실패 시 열어 둔 리스너도 함께 정리합니다.
                _eventReceiver.Stop();
                AppendLog($"[오류] 이벤트 수신 시작 실패: {ex.Message}");
                MessageBox.Show($"이벤트 수신 시작에 실패했습니다.\n{ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnToggleReceive.Enabled = true;
            }
        }

        private async void notifyTimer_Tick(object sender, EventArgs e)
        {
            if (!_eventReceiver.IsRunning)
            {
                notifyTimer.Stop();
                return;
            }

            if (!TryBuildStationConfig(out LifeSmartStationConfig config, showMessageBox: false))
            {
                return;
            }

            try
            {
                AppendLog("notify 유효시간 만료 방지를 위해 cfg:notify를 자동 갱신합니다...");
                LifeSmartPacket response = await _udpClient.RegisterNotifyAsync(
                    config,
                    txtReceiveIp.Text.Trim(),
                    _eventReceiver.ListenPort);
                AppendLog($"[NOTIFY 갱신 완료]\r\n{response.ToPrettyJson()}");
            }
            catch (Exception ex)
            {
                AppendLog($"[오류] notify 자동 갱신 실패: {ex.Message}");
            }
        }

        private void StopEventListening(bool logStopped)
        {
            notifyTimer.Stop();
            bool wasRunning = _eventReceiver.IsRunning;
            _eventReceiver.Stop();

            if (!IsDisposed)
            {
                SetEventListeningUiState(isListening: false);
            }

            if (logStopped && wasRunning)
            {
                AppendLog("실시간 이벤트 수신을 중지했습니다.");
            }
        }

        private void SetEventListeningUiState(bool isListening, string receiveIp = null, int receivePort = 0)
        {
            txtReceiveIp.Enabled = !isListening;
            txtReceivePort.Enabled = !isListening;
            btnToggleReceive.Text = isListening ? "이벤트 수신 중지" : "이벤트 수신 시작";

            if (isListening)
            {
                lblReceiveStatus.Text = $"상태: 수신 중 ({receiveIp}:{receivePort}, 240초마다 갱신)";
                lblReceiveStatus.ForeColor = Color.DarkGreen;
            }
            else
            {
                lblReceiveStatus.Text = "상태: 대기 중 (Windows 방화벽 UDP 허용 필요)";
                lblReceiveStatus.ForeColor = Color.DimGray;
            }
        }

        private void EventReceiver_EventReceived(object sender, LifeSmartEventReceivedArgs e)
        {
            AppendLog($"[EVENT <- {e.RemoteEndPoint}] ({e.Packet.PackageType})\r\n{e.Packet.ToPrettyJson()}");
        }

        private void EventReceiver_ErrorOccurred(object sender, string errorMessage)
        {
            AppendLog($"[EVENT 오류] {errorMessage}");
        }

        #endregion

        #region 공통 헬퍼 메서드

        /// <summary>
        /// 공통 비동기 API 요청 실행 래퍼입니다.
        /// 요청 중 버튼을 일시 비활성화하고, 응답 JSON 또는 오류 내용을 로그창에 출력합니다.
        /// </summary>
        private async Task ExecuteStationRequestAsync(
            Button triggerButton,
            string actionDescription,
            Func<LifeSmartStationConfig, Task<LifeSmartPacket>> requestFunc)
        {
            if (!TryBuildStationConfig(out LifeSmartStationConfig config))
            {
                return;
            }

            triggerButton.Enabled = false;
            try
            {
                AppendLog($"요청 실행: {actionDescription}");
                LifeSmartPacket response = await requestFunc(config);
                AppendLog($"[응답 수신]\r\n{response.ToPrettyJson()}");
            }
            catch (Exception ex)
            {
                AppendLog($"[오류] {actionDescription} 실패: {ex.Message}");
            }
            finally
            {
                triggerButton.Enabled = true;
            }
        }

        /// <summary>
        /// UI 입력란의 Smart Station 접속 정보를 검증하고 설정 객체를 생성합니다.
        /// </summary>
        private bool TryBuildStationConfig(out LifeSmartStationConfig config, bool showMessageBox = true)
        {
            config = null;

            if (!IPAddress.TryParse(txtStationIp.Text.Trim(), out IPAddress stationIp))
            {
                if (showMessageBox)
                {
                    MessageBox.Show("올바른 Smart Station IP를 입력하세요.", "Station Info 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return false;
            }

            string model = txtModel.Text.Trim();
            string token = txtToken.Text.Trim();
            if (string.IsNullOrEmpty(model) || string.IsNullOrEmpty(token))
            {
                if (showMessageBox)
                {
                    MessageBox.Show("LifeSmart에서 발급받은 Model과 Token을 입력하세요.", "Station Info 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return false;
            }

            if (!int.TryParse(txtLocalPort.Text.Trim(), out int localPort) || localPort < 0 || localPort > 65535)
            {
                if (showMessageBox)
                {
                    MessageBox.Show("Local Port는 0~65535 사이의 정수여야 합니다.\n(권장: 12346, 자동 할당: 0)", "Station Info 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return false;
            }

            config = new LifeSmartStationConfig(
                stationAddress: stationIp,
                model: model,
                token: token,
                stationPort: LifeSmartStationConfig.DefaultStationPort,
                localPort: localPort);
            return true;
        }

        /// <summary>
        /// 현재 PC에서 실제로 사용 중인 사설 IPv4 주소를 조회합니다.
        /// (루프백/비활성/가상 어댑터를 최대한 제외하고 기본 게이트웨이가 있는 어댑터를 우선 선택합니다.)
        /// </summary>
        private static string GetPreferredLocalIPv4()
        {
            try
            {
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

                // 1순위: 정상 동작 중이며 기본 게이트웨이가 설정된 Ethernet / Wi-Fi 어댑터
                foreach (NetworkInterface ni in interfaces.Where(n =>
                    n.OperationalStatus == OperationalStatus.Up &&
                    n.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                    n.NetworkInterfaceType != NetworkInterfaceType.Tunnel))
                {
                    IPInterfaceProperties props = ni.GetIPProperties();
                    if (props.GatewayAddresses.Count == 0)
                    {
                        continue;
                    }

                    UnicastIPAddressInformation ipv4 = props.UnicastAddresses
                        .FirstOrDefault(a => a.Address.AddressFamily == AddressFamily.InterNetwork);
                    if (ipv4 != null)
                    {
                        return ipv4.Address.ToString();
                    }
                }

                // 2순위: DNS 조회를 통한 첫 번째 IPv4 주소
                IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress fallback = hostEntry.AddressList
                    .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
                return fallback != null ? fallback.ToString() : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 어떤 스레드에서 호출되더라도 안전하게 로그 창에 타임스탬프와 메시지를 추가합니다.
        /// </summary>
        private void AppendLog(string message)
        {
            if (IsDisposed || !IsHandleCreated)
            {
                return;
            }

            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new Action<string>(AppendLog), message);
                }
                catch (ObjectDisposedException)
                {
                    // 폼 종료 중 발생하는 비동기 호출 무시
                }
                return;
            }

            // 로그가 너무 길어지면 오래된 앞부분을 정리합니다.
            if (rtbLog.TextLength > MaxLogCharacters)
            {
                rtbLog.Clear();
            }

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            rtbLog.AppendText($"[{timestamp}] {message}{Environment.NewLine}");
            rtbLog.SelectionStart = rtbLog.TextLength;
            rtbLog.ScrollToCaret();
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            rtbLog.Clear();
        }

        #endregion
    }
}
