# LifeSmart API 샘플 소스 저장소 (`ulroot-sample-source`)

이 저장소는 **LifeSmart IoT / Smart Station API**를 연동하려는 개발자들이 빠르게 프로토콜을 이해하고 개발에 참고할 수 있도록 제공하는 **공식 샘플 소스 모음**입니다.

각 연동 방식(Local UDP, Cloud API 등) 및 언어/플랫폼별 샘플은 독립된 하위 폴더로 구성되며, 폴더마다 개별 `README.md`와 실행 가능한 샘플 프로젝트를 제공합니다.

---

## 저장소 구조

```text
ulroot-sample-source/
├── README.md                 # 저장소 전체 개요 및 샘플 목록 (현재 문서)
├── local-UDP-sample/         # C# WinForms 기반 LifeSmart Smart Station Local UDP(OpenDev) 샘플
│   ├── README.md             # 해당 샘플의 상세 설명 및 사용 가이드
│   └── local-UDP-sample.sln
├── cloud-api-sample/         # C# WinForms 기반 LifeSmart Cloud OpenAPI & WebSocket 샘플
│   ├── README.md             # 해당 샘플의 상세 설명 및 사용 가이드
│   └── cloud-api-sample.sln
└── (추가 샘플 프로젝트 예정)
```

---

## 샘플 프로젝트 목록

| 샘플 폴더 | 프로토콜 / API | 언어 / 프레임워크 | 설명 |
|---|---|---|---|
| [`local-UDP-sample`](./local-UDP-sample) | Smart Station Local UDP (OpenDev) | C# (.NET Framework 4.8, WinForms) | 로컬 네트워크(LAN)에서 Smart Station 검색, 디바이스 조회/제어, 설정 명령, 실시간 이벤트(Notify) 수신 샘플 |
| [`cloud-api-sample`](./cloud-api-sample) | Cloud Platform OpenAPI (HTTPS) & WebSocket (WSS) | C# (.NET Framework 4.8, WinForms) | OAuth 인증 URL 팝업, 토큰 갱신, `EpGetAll`/`EpGet`/`EpSet`, `SceneGet`/`SceneSet`, WebSocket 실시간 이벤트 수신 샘플 |

> 추후 다양한 LifeSmart API 연동 샘플이 동일한 폴더 구조로 추가될 예정입니다.

---

## 공통 사전 준비 사항

LifeSmart API(Local OpenDev 또는 Cloud API)를 사용하려면 사전에 LifeSmart(또는 담당 파트너)로부터 연동 인증 정보(Local API: `Model`/`Token`, Cloud API: `AppKey`/`AppToken` 등)를 발급받아야 합니다.

각 샘플의 상세한 사전 설정, 실행 방법, 프로토콜 명세 요약은 해당 샘플 폴더 내부의 `README.md`를 참고해 주세요.

---

## 보안 주의 사항

- 실제 운영 환경에서 발급받은 `Model`, `Token`, `AppKey`, `AppToken`, `UserToken` 등의 인증 정보는 소스 코드에 하드코딩하거나 Git 저장소에 커밋하지 마세요.
- 샘플 프로젝트들은 실행 화면(UI)이나 환경 설정을 통해 인증 정보를 직접 입력하도록 구성되어 있습니다.
