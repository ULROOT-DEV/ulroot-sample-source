# cloud-api-sample

**LifeSmart Cloud Platform OpenAPI (HTTPS) 및 WebSocket Event**를 C# (.NET Framework 4.8 / WinForms)으로 구현한 샘플 프로젝트입니다.

클라우드 서버를 통해 **1단계(선택) OAuth 사용자 인증 URL 팝업으로 `UserToken`을 발급받거나, 이미 보유한 `UserToken`을 2단계에 바로 입력하여 디바이스 조회·제어, 씬 조회·실행, WebSocket 실시간 이벤트 수신**을 테스트할 수 있습니다.

---

## 주요 기능

1. **1단계 (선택) — 3.1 사용자 OAuth 인증 URL 팝업 (`auth.authorize`)**
   - `UserToken`이 아직 없는 경우 최초 1회 발급받기 위한 과정입니다. **(이미 `UserToken`을 보유하고 있다면 1단계는 생략하고 2단계에 바로 입력해 사용할 수 있습니다.)**
   - `AppKey`, `AppToken`, `auth_callback`, `lang` 기반으로 MD5 서명이 포함된 인증 Request URL을 자동 생성합니다.
   - `auth_callback`이 `http://localhost:...` 형태일 경우 내장 `HttpListener`로 로컬 콜백 서버를 임시 구동하여 브라우저 로그인·승인 직후 반환되는 `userid`, `usertoken`, `svrurl`, `svrrgnid`를 자동으로 파싱해 2단계 입력란에 반영합니다.
   - 외부 `auth_callback`을 사용하는 경우 콜백 URL이나 JSON 응답을 직접 붙여넣어 파싱하는 기능도 제공합니다.
2. **2단계 — 3.4 UserToken 갱신 (`auth.refreshtoken`)**
   - 기존 `usertoken`이 만료되기 전에 새 `usertoken`으로 갱신하고 입력란에 자동 반영합니다.
3. **4.5.6 ~ 4.5.8 디바이스 조회 및 제어**
   - `EpGetAll`: 계정 내 전체 디바이스 목록 조회 (`degree` 0~3 선택 지원, 첫 번째 장비의 `agt`/`me` 자동 입력)
   - `EpGet`: 특정 Smart Station(`agt`)의 단일 디바이스(`me`) 상태 조회
   - `EpSet`: 단일 디바이스 제어 (`idx`, `type`, `val`, `tag` 지정 및 `ON`/`OFF` 프리셋 제공)
4. **4.5.10 ~ 4.5.11 씬(Scene) 조회 및 실행**
   - `SceneGet`: Smart Station(`agt`)의 씬 목록 조회 (첫 번째 씬 `id` 자동 입력)
   - `SceneSet`: 씬 실행 (`groupsw`/`grouphw`/`grouprgbw`용 선택 파라미터 `type`, `RGBW` 지원)
5. **7장 WebSocket 실시간 이벤트 수신 (`WSS`)**
   - 리전(`svrrgnid`)에 맞는 `wss://...` 엔드포인트에 연결한 직후 **`WbAuth`** 인증 메시지를 자동 전송합니다.
   - 연결 유지 중 서버에서 푸시되는 실시간 이벤트(`type="io"`, 온라인/오프라인, AI 이벤트, 보안 `elog`, 하트비트 등)를 로그 창에 실시간 출력합니다.
   - 연결 해제 시 **`RmAuth`** 인증 해제 메시지를 전송한 뒤 정상 종료합니다.

---

## 단계별 입력 항목 안내

### 기본 앱 설정 (공통 필수)
| 항목 | 설명 |
|---|---|
| `AppKey` | LifeSmart Open Platform에서 발급받은 앱 키 (모든 인증·OpenAPI·WebSocket 서명에 필수) |
| `AppToken` | LifeSmart Open Platform에서 발급받은 앱 토큰 (서명 계산용, 외부 유출 주의) |

### 1단계 (선택): UserToken 발급 (`3.1 auth.authorize`)
- `UserToken`이 아직 없는 경우에만 **[3.1 인증 URL 팝업 (UserToken 발급)]** 버튼을 눌러 사용자 로그인·승인을 진행합니다.
- **기존에 이미 발급받은 `UserToken`이 있다면 1단계는 필요 없으며, 아래 2단계에 바로 입력해 사용할 수 있습니다.**

### 2단계: UserToken 및 서버 정보 (기존 UserToken 직접 입력 또는 1단계 결과 자동 반영)
| 항목 | 설명 |
|---|---|
| `UserID (숫자)` (`userid`) | 사용자 인증 후 발급받은 숫자 형식의 LifeSmart 사용자 ID (`user_no`) |
| `UserToken` | 사용자 인증 후 발급받은 액세스 토큰 |
| `Region` / `API URL` / `WS URL` | 계정 리전(`svrrgnid`)에 맞는 OpenAPI 및 WebSocket 주소. 한국(`kr`) 계정은 기본 **`AME`**(`https://api.us.ilifesmart.com/app/`, `wss://api.us.ilifesmart.com:8443/wsapp/`) 사용 |

---

## 프로젝트 구조

```text
cloud-api-sample/
├── cloud-api-sample.sln                 # Visual Studio 솔루션 파일
├── cloud-api-sample.csproj              # .NET Framework 4.8 WinForms 프로젝트
├── Program.cs                           # 진입점 (TLS 1.2 활성화 포함)
├── MainForm.cs / Designer.cs / .resx    # 메인 UI (1단계/2단계 인증 + OpenAPI + WebSocket)
├── AuthPopupForm.cs / Designer.cs/.resx # 3.1 auth.authorize URL 팝업 및 콜백 파서
├── Protocol/
│   ├── CloudRegionInfo.cs               # svrrgnid별 OpenAPI / WebSocket URL 매핑 (Appendix 2 & 7.2)
│   ├── CloudApiConfig.cs                # 인증 및 서버 URL 설정 모델
│   └── CloudApiSigner.cs                # 3.2 OAuth / 3.4 Refresh / 4.2 OpenAPI & WS MD5 서명 생성기
└── Client/
    ├── LifeSmartCloudClient.cs          # HttpClient 기반 비동기 OpenAPI 클라이언트
    └── LifeSmartWebSocketClient.cs      # ClientWebSocket 기반 실시간 이벤트 클라이언트
```

---

## 프로토콜 및 서명 규칙 요약

### 1. 사용자 인증 URL 서명 (매뉴얼 3.2)
- `apptoken`을 제외한 파라미터를 키 오름차순으로 연결하고 마지막에 `apptoken`을 붙인 뒤 MD5(32자리 소문자 16진수) 계산 (`lang`은 서명 제외, `auth_callback`은 URL 인코딩 전 원문 사용):
```text
appkey=<appkey>&auth_callback=<auth_callback>&time=<time>&apptoken=<apptoken>
```

### 2. OpenAPI / WebSocket(`WbAuth`, `RmAuth`) 서명 (매뉴얼 4.2.1 / 7.3)
- `params`의 모든 키를 ASCII 오름차순으로 정렬한 뒤 아래 형식으로 조합해 MD5(32자리 소문자 16진수) 계산:
```text
method:<method>[,<param1>:<val1>,...],time:<time>,userid:<userid>,usertoken:<usertoken>,appkey:<appkey>,apptoken:<apptoken>
```

### 3. 리전(`svrrgnid`)별 서버 주소 (매뉴얼 7.2 / Appendix 2)

| `svrrgnid` | OpenAPI URL (`svrurl`) | WebSocket URL |
|---|---|---|
| `AME` (미국/한국) | `https://api.us.ilifesmart.com/app/` | `wss://api.us.ilifesmart.com:8443/wsapp/` |
| `GS` (글로벌 기본) | `https://api.ilifesmart.com/app/` | `wss://api.ilifesmart.com:8443/wsapp/` |
| `APZ` (아시아태평양) | `https://api.apz.ilifesmart.com/app/` | `wss://api.apz.ilifesmart.com:8443/wsapp/` |
| `EUROPE` (유럽) | `https://api.eur.ilifesmart.com/app/` | `wss://api.eur.ilifesmart.com/wsapp/` |
| `JAP` (일본) | `https://api.jp.ilifesmart.com/app/` | `wss://api.jp.ilifesmart.com:8443/wsapp/` |
| `VIP1` | `https://api.cn1.ilifesmart.com/app/` | `wss://api.cn1.ilifesmart.com:8443/wsapp/` |
| `CN2` | `https://api.cn2.ilifesmart.com/app/` | `wss://api.cn2.ilifesmart.com:8443/wsapp/` |

---

## 빌드 및 실행 방법

### 요구 환경
- Windows OS
- Visual Studio 2019 이상 (또는 MSBuild)
- .NET Framework 4.8 Developer Pack
- NuGet 패키지: `Newtonsoft.Json` (13.0.3)

### Visual Studio에서 빌드
1. `cloud-api-sample.sln`을 Visual Studio로 엽니다.
2. 필요 시 솔루션 우클릭 → **NuGet 패키지 복원(Restore NuGet Packages)**을 실행합니다.
3. 구성(`Debug` 또는 `Release`)을 선택한 뒤 빌드(`Ctrl + Shift + B`) 및 실행(`F5`)합니다.

### 명령줄(MSBuild)로 빌드
```powershell
# (최초 1회) NuGet 패키지 복원
nuget restore cloud-api-sample.sln

# Debug 빌드
msbuild cloud-api-sample.sln /p:Configuration=Debug /p:Platform="Any CPU"

# Release 빌드
msbuild cloud-api-sample.sln /p:Configuration=Release /p:Platform="Any CPU"
```

빌드된 실행 파일은 각각 아래 경로에 생성됩니다.
- Debug: `cloud-api-sample\bin\Debug\cloud-api-sample.exe`
- Release: `cloud-api-sample\bin\Release\cloud-api-sample.exe`

---

## 자주 발생하는 오류 코드 (매뉴얼 4.3)

- `10004` (`Invalid signature`): `AppKey`/`AppToken`/`UserToken` 오타, 또는 PC 시스템 시각이 UTC 기준 5분 이상 차이나는지 확인하세요.
- `10005` / `10006`: 사용자 인증이 되어 있지 않거나 `UserToken`이 만료되었습니다. `[3.4 토큰 갱신]` 또는 `[3.1 인증 URL 팝업]`으로 재인증하세요.
- `10022` (`The request address need to be redirected`): 계정의 실제 리전(`svrrgnid`)과 선택한 `API URL` / `WS URL`이 다릅니다. 인증 시 받은 `svrurl`/`svrrgnid`에 맞는 리전을 선택하세요.
