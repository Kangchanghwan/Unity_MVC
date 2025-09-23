# EventSystem - Unity 이벤트 기반 2D 게임

Unity 6000.2.4f1을 사용한 2D 게임 프로젝트로, 포괄적인 이벤트 시스템과 MVC 아키텍처를 구현합니다.

## 📋 프로젝트 개요

이 프로젝트는 현대적인 Unity 개발 패턴을 보여주는 예제 게임입니다:
- **이벤트 기반 아키텍처**: 글로벌 및 로컬 이벤트 시스템
- **MVC 패턴**: Model, View, Controller 완전 분리
- **비동기 프로그래밍**: UniTask를 활용한 상태 머신
- **컴포넌트 기반 설계**: 재사용 가능한 모듈형 구조

## 🛠 기술 스택

- **Unity**: 6000.2.4f1
- **UniTask**: 비동기 작업 처리
- **Unity Input System**: 현대적인 입력 처리
- **Universal Render Pipeline (URP)**: 고성능 렌더링
- **Unity 2D**: 2D 게임 개발 도구

## 🏗 아키텍처

### 이벤트 시스템
- **글로벌 이벤트**: `EventManager`를 통한 시스템 간 통신
- **로컬 이벤트**: MonoBehaviour 계층 기반 이벤트 버블링
- **타입 안전성**: 제네릭을 활용한 컴파일 타임 검증

### MVC 패턴
- **Model**: 순수 C# 클래스, 비즈니스 로직 담당
- **View**: MonoBehaviour 컴포넌트, 시각적 표현 담당
- **Controller**: Model과 View 중재, 이벤트 처리

### 상태 머신
- UniTask 기반 비동기 상태 전환
- 체인 방식의 상태 관리
- 로깅 지원으로 디버깅 용이

## 📁 프로젝트 구조

```
Assets/Scripts/
├── Common/                     # 공통 시스템
│   ├── EventSystem.cs         # 이벤트 시스템 핵심
│   └── StateMachineSystem.cs  # 상태 머신
├── Feature/
│   ├── Main/                  # 메인 화면
│   │   ├── Controller/        # 메인 컨트롤러
│   │   ├── Model/            # 점수 모델
│   │   └── View/             # UI 뷰
│   └── Game/                  # 게임플레이
│       ├── Controller/        # 게임 컨트롤러
│       │   ├── Player/       # 플레이어 관련
│       │   └── Bullet/       # 총알 관련
│       ├── Model/            # 게임 데이터
│       ├── View/             # 게임 뷰
│       └── Events/           # 게임 이벤트
```

## 🚀 주요 기능

### 이벤트 시스템
```csharp
// 이벤트 정의
public struct PlayerJumpEvent : IEvent { }

// 글로벌 이벤트 구독
EventManager.Subscribe<PlayerJumpEvent>(OnPlayerJump);

// 이벤트 발행
this.EmitGlobal<PlayerJumpEvent>();
```

### 상태 머신
```csharp
// 상태 정의
public class IdleState : PlayerStateBase
{
    public async UniTask<PlayerStateBase> Enter(PlayerStateBase previousState)
    {
        // 상태 로직
        return await SomeCondition() ? new JumpState() : null;
    }
}

// 상태 머신 실행
_stateMachine.Execute<PlayerStateBase>(new IdleState()).Forget();
```

### MVC 패턴
```csharp
// Model: 순수 C# 로직
public class PlayerModel
{
    public event Action<int, int> OnHealthChanged;
    public void ApplyDamage(int damage) { /* 로직 */ }
}

// Controller: 중재자 역할
public class PlayerController : EventListenerMono
{
    private PlayerModel _model;
    private PlayerView _view;
    
    public override EventChain OnEventHandle(IEvent @event) { /* 이벤트 처리 */ }
}

// View: 시각적 표현
public class PlayerView : MonoBehaviour
{
    public void PlayJumpAnimation() { /* 애니메이션 */ }
}
```

## 🎮 게임 시스템

### 플레이어 시스템
- 체력 관리 및 데미지 처리
- 점프 및 이동 기능
- 상태 기반 애니메이션

### 총알 시스템
- 물리 기반 발사체
- 충돌 감지 및 처리
- 이펙트 및 사운드

### UI 시스템
- 점수 표시
- 체력 UI
- 메인 메뉴

## 🛡 설계 원칙

1. **이벤트 기반**: 모든 시스템 간 통신은 이벤트 사용
2. **관심사 분리**: MVC 패턴의 엄격한 경계 유지
3. **비동기 우선**: UniTask를 활용한 논블로킹 코드
4. **컴포지션**: 상속보다 컴포넌트 기반 설계
5. **타입 안전성**: 제네릭과 인터페이스 활용

## 🔧 개발 환경 설정

1. Unity 6000.2.4f1 이상 설치
2. 프로젝트 복제 후 Unity에서 열기
3. Package Manager에서 필요한 패키지 자동 설치
4. Build Settings에서 플랫폼 설정

## 📚 참고 자료

- [CLAUDE.md](CLAUDE.md): 개발자를 위한 상세 가이드
- Unity 공식 문서: https://docs.unity3d.com/
- UniTask: https://github.com/Cysharp/UniTask

## 🤝 기여 방법

1. 이슈 생성 및 토론
2. 기능 브랜치 생성
3. MVC 패턴 및 이벤트 시스템 준수
4. 풀 리퀘스트 제출

## 📄 라이선스

이 프로젝트는 학습 목적으로 제작되었습니다.