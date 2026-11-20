namespace LocalUdpSample.Protocol
{
    /// <summary>
    /// LifeSmart Local UDP 프로토콜의 메시지 타입(pkg_type) 정의입니다.
    /// (매뉴얼 3.1 Message Format 참조)
    /// </summary>
    public enum LifeSmartPackageType : ushort
    {
        /// <summary>예약됨</summary>
        Reserved = 0,

        /// <summary>조회 요청 (Device -> Smart Station)</summary>
        Get = 1,

        /// <summary>조회 응답 (Smart Station -> Device)</summary>
        GetReply = 2,

        /// <summary>제어/설정 요청 (Device -> Smart Station)</summary>
        Set = 3,

        /// <summary>제어/설정 응답 (Smart Station -> Device)</summary>
        SetReply = 4,

        /// <summary>추가 요청</summary>
        Add = 5,

        /// <summary>추가 응답</summary>
        AddReply = 6,

        /// <summary>삭제 요청</summary>
        Delete = 7,

        /// <summary>삭제 응답</summary>
        DeleteReply = 8,

        /// <summary>로컬 이벤트 알림 (Smart Station -> Device)</summary>
        Notify = 9,

        /// <summary>로컬 이벤트 알림 응답</summary>
        NotifyReply = 10
    }
}
