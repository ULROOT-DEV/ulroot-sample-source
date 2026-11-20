# local-UDP-sample

**LifeSmart Smart Station Local Interface (OpenDev) UDP API**를 C# (.NET Framework 4.8 / WinForms)으로 구현한 샘플 프로젝트입니다.

클라우드 서버를 거치지 않고 동일 로컬 네트워크(LAN) 내에서 Smart Station과 직접 UDP 통신하여 **스테이션 검색(Discovery), 서브 디바이스 조회 및 제어, 설정 명령, 실시간 이벤트 수신**을 테스트하고 코드 구조를 참고할 수 있습니다.

---

## 주요 기능

1. **Smart Station 자동 검색 (Discovery)**
   - UDP `12345` 포트로 `Z-SEARCH * \r\n` 브로드캐스트를 전송해 동일 LAN 내의 Smart Station IP 및 `LSID`를 자동 검색합니다.
2. **조회 / 설정 명령 (`GET` / `SET config`)**
   - `GET eps`: Smart Station에 연결된 전체 서브 디바이스 목록 조회
   - `GET scene`: Smart Station에 등록된 씬(Scene) 목록 조회
   - `SET config (cfg:getver)`: Smart Station 펌웨어 버전 및 모델 타입 조회
   - `SET config (cfg:reboot)`: Smart Station 재부팅 (실행 전 확인 팝업 표시)
3. **단일 서브 디바이스 제어 (`SET ep`)**
   - `me`(디바이스 ID), `idx`(IO 인덱스), `type`, `val`을 지정하여 스위치/소켓 등의 디바이스 제어 테스트 (`ON`/`OFF` 프리셋 버튼 제공)
4. **실시간 로컬 이벤트 수신 (`Local Event Service`)**
   - `SET config (cfg:notify)` 명령으로 본 PC의 IP와 수신 포트를 Smart Station에 등록하고, 서브 디바이스 상태 변경·추가·삭제·온라인/오프라인 이벤트를 실시간 수신합니다.
   - Smart Station의 `notify` 등록은 **300초 후 만료**되므로, 샘플에서는 **240초마다 자동으로 `cfg:notify`를 재전송**하여 연결을 유지합니다.

---

## 사전 준비 사항

### 1. OpenDev 인터페이스 활성화 및 인증 정보 발급
LifeSmart로부터 발급받은 **`Model`**과 **`Token`**이 필요하며, Smart Station에서 OpenDev 로컬 인터페이스가 활성화되어 있어야 합니다.

- **앱 템플릿 활성화 방식 (App Enabling)**: LifeSmart 앱의 OpenDev 템플릿으로 설정한 경우, Smart Station은 응답을 클라이언트 **UDP `12346` 포트**로 고정 반환합니다.
- **QR 코드 스캔 활성화 방식 (Scanning Code)**: QR 코드로 활성화한 경우, Smart Station은 클라이언트가 요청을 보낸 송신 포트로 응답을 반환합니다.

> 본 샘플은 기본 로컬 포트를 **`12346`**으로 설정하여 두 활성화 방식 모두에서 바로 동작하도록 구성되어 있습니다.

### 2. 네트워크 및 방화벽 설정
- PC와 LifeSmart Smart Station이 **동일한 로컬 네트워크(서브넷)**에 연결되어 있어야 합니다.
- PC의 시스템 시각과 Smart Station의 시각 차이가 **5분을 초과**하면 서명 검증(`ts`) 단계에서 요청이 거부됩니다.
- 실시간 이벤트 수신(`cfg:notify`) 및 응답 수신을 위해 **Windows 방화벽에서 사용하는 UDP 포트(기본 `12346`, 이벤트 수신 포트 예: `12350`)의 인바운드를 허용**해야 합니다.

---

## 프로젝트 구조

실제 프로젝트에 바로 재사용할 수 있도록 **프로토콜 계층(`Protocol/`)**, **통신 클라이언트 계층(`Client/`)**, **UI 계층(`MainForm`)**을 분리해 두었습니다.

```text
local-UDP-sample/
├── local-UDP-sample.sln               # Visual Studio 솔루션 파일
├── local-UDP-sample.csproj            # .NET Framework 4.8 WinForms 프로젝트
├── Program.cs                         # 애플리케이션 진입점
├── MainForm.cs / Designer.cs / .resx  # 테스트용 WinForms UI
├── Protocol/
│   ├── LifeSmartPackageType.cs        # 패킷 타입 열거형 (GET=1, SET=3, NOTIFY=9 등)
│   ├── LifeSmartPacket.cs             # 10바이트 바이너리 헤더 + UTF-8 JSON 인코딩/디코딩
│   └── LifeSmartSigner.cs             # MD5 서명 생성기 (args 키 오름차순 정렬 및 배열 예외 처리)
└── Client/
    ├── LifeSmartStationConfig.cs      # Smart Station 접속 정보 모델
    ├── LifeSmartUdpClient.cs          # 비동기 UDP 요청/응답 클라이언트 (타임아웃 지원)
    ├── LifeSmartDiscoveryClient.cs    # UDP 12345 브로드캐스트 자동 검색 클라이언트
    └── LifeSmartEventReceiver.cs      # 비동기 UDP 실시간 이벤트 수신 리스너
```

---

## 프로토콜 핵심 요약

### 1. UDP 패킷 포맷 (10바이트 헤더 + UTF-8 JSON 본문)

| 필드 | 크기 | 형식 | 설명 |
|---|---|---|---|
| `header` | 2 bytes | ASCII | 고정 시작 식별자 `"JL"` (`0x4A`, `0x4C`) |
| `version` | 2 bytes | Big-Endian uint16 | 프로토콜 버전 |
| `pkg_type` | 2 bytes | Big-Endian uint16 | `1`: GET, `2`: GET-REPLY, `3`: SET, `4`: SET-REPLY, `9`: NOTIFY 등 |
| `pkg_size` | 4 bytes | Big-Endian uint32 | JSON 본문의 UTF-8 바이트 길이 |
| `content` | `pkg_size` bytes | UTF-8 String | JSON 페이로드 |

### 2. MD5 서명(`sys.sign`) 규칙
1. `args`가 JSON 객체(`JObject`)인 경우, **모든 키를 ASCII 오름차순으로 정렬**해 `key:value` 형태로 이어 붙입니다.
2. `args`가 비어 있거나 배열(`JArray`, 예: 다중 디바이스 제어 `SET eps`)인 경우, `args`는 서명 문자열에 포함하지 않습니다.
3. 아래 형식으로 원문 서명 문자열을 조합한 뒤 **UTF-8 MD5 해시(32자리 소문자 16진수)**를 계산합니다.

```text
obj:<obj>,<arg1>:<val1>,...,<argN>:<valN>,ts:<utc_seconds>,model:<model>,token:<token>
```

> 구현 예시는 `Protocol/LifeSmartSigner.cs`를 참고하세요.

---

## 빌드 및 실행 방법

### 요구 환경
- Windows OS
- Visual Studio 2019 이상 (또는 MSBuild)
- .NET Framework 4.8 Developer Pack
- NuGet 패키지: `Newtonsoft.Json` (13.0.3)

### Visual Studio에서 빌드
1. `local-UDP-sample.sln`을 Visual Studio로 엽니다.
2. 필요 시 NuGet 패키지 복원(Restore NuGet Packages)을 실행합니다.
3. 구성(`Debug` 또는 `Release`)을 선택한 뒤 빌드(`Ctrl + Shift + B`) 및 실행(`F5`)합니다.

### 명령줄(MSBuild)로 빌드
```powershell
# (최초 1회) NuGet 패키지 복원
nuget restore local-UDP-sample.sln

# Debug 빌드
msbuild local-UDP-sample.sln /p:Configuration=Debug /p:Platform="Any CPU"

# Release 빌드
msbuild local-UDP-sample.sln /p:Configuration=Release /p:Platform="Any CPU"
```

빌드된 실행 파일은 각각 아래 경로에 생성됩니다.
- Debug: `local-UDP-sample\bin\Debug\local-UDP-sample.exe`
- Release: `local-UDP-sample\bin\Release\local-UDP-sample.exe`

---

## 문제 해결 (Troubleshooting)

- **응답 시간 초과 (`TimeoutException`)가 발생하는 경우**
  - Smart Station IP가 정확한지 확인하세요 (`[자동 검색]` 버튼으로 LAN 내 IP 확인 가능).
  - Smart Station에서 OpenDev가 활성화되어 있는지 확인하세요.
  - 앱 템플릿 방식 사용 시 `Local Port`가 `12346`으로 되어 있는지, Windows 방화벽이 UDP `12346` 포트를 차단하고 있지 않은지 확인하세요.
- **서명 오류(0이 아닌 `code` 응답)가 발생하는 경우**
  - `Model`과 `Token` 값에 오타나 앞뒤 공백이 없는지 확인하세요.
  - PC의 현재 시간(UTC 기준)과 Smart Station의 시간 차이가 5분 이내인지 확인하세요.
- **이벤트 수신을 시작했는데 이벤트가 들어오지 않는 경우**
  - `수신 IP`가 Smart Station과 같은 네트워크 대역의 PC IPv4 주소인지 확인하세요.
  - Windows 방화벽 인바운드 규칙에서 `수신 Port`(기본 `12350`) UDP 통신을 허용했는지 확인하세요.
