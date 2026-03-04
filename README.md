# 🛸projectlup (Project-LUP)

> **인하대학교 미래인재개발원 게임잼 1등 수상작** > **개발 기간**: 202X.XX.XX ~ 202X.XX.XX (2일)  
> **담당 역할**: 메인 개발 (몬스터 AI 시스템, 플레이어 전투 시스템, 사운드 및 시스템 아키텍처)

---

## 🎯 프로젝트 목표
- **협업 및 소통**: GitHub Desktop을 활용한 실전 협업 및 코드 관리 경험
- **역량 발휘**: 2일 내에 완성도 높은 게임을 구현하기 위한 효율적 구조 설계
- **기술적 성장**: 인터페이스 기반의 모듈형 AI 시스템 구축

---

## 🛠 핵심 개발 상세 (Technical Deep Dive)

### 1. 인터페이스 기반 모듈형 몬스터 AI
다양한 몬스터 패턴을 단기간에 구현하기 위해 **전략 패턴(Strategy Pattern)**을 적용했습니다.

<details>
  <summary>🔍 인터페이스 구조 상세 보기</summary>

- **IWatchBehavior (경계 행동)**: `PassiveWatch` 클래스에서 플레이어를 응시하며 경계하는 로직 구현
- **IPlayerDetector (감지 로직)**: `HorizontalRangeDetector` 등을 통해 층간 오작동 방지
- **IAttackBehavior (공격 행동)**: `ProjectileAttack`으로 원거리 공격 로직 분리

```cs
// 예시: 경계 행동 인터페이스 구현
public class PassiveWatch : IWatchBehavior {
    // ... 코드 생략
}
