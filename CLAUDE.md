# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## 프로젝트 개요

Unity 6000.2.4f1을 사용한 2D 게임 프로젝트로, 이벤트 기반 MVC 아키텍처를 구현하고 있습니다. 로컬(MonoBehaviour 기반)과 글로벌 이벤트 처리를 모두 지원하는 포괄적인 이벤트 시스템을 제공합니다.

## 주요 의존성

- **UniTask**: Unity용 async/await 지원 (Git URL로 가져옴)
- **Unity Input System**: 현대적인 입력 처리
- **Unity 2D Feature**: 2D 게임 개발 도구
- **Universal Render Pipeline (URP)**: 렌더링 파이프라인

## 핵심 아키텍처

### 이벤트 시스템 (`Assets/Scripts/Common/EventSystem.cs`)
프로젝트는 이중 레이어 이벤트 시스템을 사용합니다:

1. **글로벌 이벤트**: `EventManager` 정적 클래스로 관리
   - `IEventListener` 인터페이스와 `Action<T>` 구독 모두 지원
   - `EventManager.Publish<T>()`로 타입 안전한 이벤트 발행
   - `EventListenerMono` 베이스 클래스를 통한 자동 정리

2. **로컬 이벤트**: `IMonoEventDispatcher`를 통한 MonoBehaviour 계층 기반
   - `GetComponentsInParent<EventListenerMono>()`를 통해 부모 GameObject로 이벤트 버블링
   - 확장 메서드로 로컬 전용, 글로벌 전용, 또는 둘 다 지원 (`Emit`, `EmitGlobal`, `EmitBoth`)

### 상태 머신 (`Assets/Scripts/Common/StateMachineSystem.cs`)
- UniTask를 사용한 비동기 상태 머신
- 상태는 `Enter`와 `Exit` 메서드가 있는 `IState<T>` 구현
- 상태 전환에 대한 선택적 로깅 지원

### MVC 패턴
프로젝트는 엄격한 MVC 분리를 따릅니다:

- **Model**: 비즈니스 로직을 가진 순수 C# 클래스 (예: `PlayerModel`, `BulletModel`)
  - C# 이벤트를 사용하여 컨트롤러에 상태 변화 알림
  - Unity 의존성 없음
  
- **View**: 프레젠테이션을 처리하는 MonoBehaviour 컴포넌트 (예: `PlayerView`, `BulletView`)
  - 애니메이션, 물리, 시각 효과 관리
  - 비즈니스 로직 없음

- **Controller**: Model과 View 사이의 다리 역할 (예: `PlayerController`, `BulletController`)
  - 글로벌 이벤트 처리를 위해 `EventListenerMono` 상속
  - 이벤트 발행을 위해 `IMonoEventDispatcher` 구현
  - 상태 머신 관리 및 Model-View 상호작용 조정

## 프로젝트 구조

```
Assets/Scripts/
├── Common/                    # 공유 시스템
│   ├── EventSystem.cs        # 글로벌 및 로컬 이벤트 시스템
│   └── StateMachineSystem.cs # 비동기 상태 머신
├── Feature/
│   ├── Main/                 # 메인 메뉴/UI 기능
│   │   ├── Controller/       # 메인 컨트롤러
│   │   ├── Model/           # 점수 및 게임 상태
│   │   └── View/            # UI 컴포넌트
│   └── Game/                # 핵심 게임플레이 기능
│       ├── Controller/      # 게임 로직 컨트롤러
│       │   ├── Player/      # 플레이어 전용 컨트롤러
│       │   └── Bullet/      # 총알 전용 컨트롤러
│       ├── Model/          # 게임 데이터 모델
│       ├── View/           # 게임 프레젠테이션
│       └── Events/         # 게임 전용 이벤트
```

## 개발 패턴

### 이벤트 정의
이벤트는 `IEvent`를 구현하는 구조체로 정의됩니다:
```csharp
public struct PlayerJumpEvent : IEvent { }

public struct PlayerHealthChangedEvent : IEvent
{
    public int Current { get; }
    public int Max { get; }
    // 검증이 포함된 생성자
}
```

### 컨트롤러 패턴
컨트롤러는 다음 구조를 따릅니다:
1. `EventListenerMono` 상속 및 `IMonoEventDispatcher` 구현
2. `Awake()`에서 Model과 View 초기화
3. `Start()`에서 Model C# 이벤트와 글로벌 이벤트 구독
4. `OnEventHandle()` 오버라이드에서 이벤트 처리
5. `OnDestroy()`에서 구독 정리

### 상태 구현
상태는 `IState<TState>`를 구현하는 클래스입니다:
- `Enter()` 메서드에서 다음 상태 반환 (상태 머신 종료 시 null)
- `StateMachine.Execute()`를 사용하여 상태 체인 실행
- 상태는 액션을 위해 컨트롤러를 참조할 수 있음

### Null 안전성
- 필수 컴포넌트에 대한 null 체크는 `Debug.Assert()` 사용
- `Awake()`에서 `GetComponent<>()` 호출로 폴백 제공

## 일반적인 개발 명령어

Unity 프로젝트는 일반적으로 명령줄 빌드 도구를 사용하지 않습니다. 개발은 Unity 에디터를 통해 수행됩니다:

- **빌드**: File → Build Settings → Build
- **테스트**: Window → General → Test Runner
- **패키지 매니저**: Window → Package Manager
- **콘솔**: Window → General → Console (디버깅용)

## 주요 설계 원칙

1. **이벤트 기반 아키텍처**: 시스템 간 모든 통신에 이벤트 사용
2. **관심사 분리**: 교차 의존성이 없는 엄격한 MVC 경계
3. **기본적으로 비동기**: 모든 비동기 작업에 UniTask 사용
4. **상속보다 컴포지션**: 컴포넌트 기반 설계 선호
5. **글로벌 vs 로컬 이벤트**: 적절한 이벤트 범위 선택 (시스템 간에는 글로벌, 계층 내에는 로컬)

## 이벤트 시스템 사용법

- **구독**: `EventManager.Subscribe<EventType>(listener)` 또는 `EventManager.Subscribe<EventType>(action)`
- **발행**: `this.EmitGlobal<EventType>()` 또는 `EventManager.Publish(eventInstance)`
- **로컬 이벤트**: 부모 계층에만 `this.Emit<EventType>()`
- **정리**: `EventListenerMono` 베이스 클래스를 통한 자동 정리