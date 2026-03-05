# Project-LUP
## 인하대학교 미래인재개발원 Unity 팀 프로젝트

### <p align="center">🎬 [플레이 영상](https://youtu.be/-ySWBjffa4c)</p>


&nbsp; &nbsp; 

## 📝 프로젝트 소개
- **게임 장르** : 3D 디펜스 슈팅 (Defense Shooting)
- **제작 기간** : 2025.3Q (3주)
- **게임 요약** : 각기 다른 능력을 지닌 캐릭터들을 이용해, 방어선을 사수하는 게임입니다.


&nbsp; &nbsp; 

## 🎯 프로젝트 목표
**공통의 세계관을 공유하며 각 파트별 독립적인 게임 콘텐츠를 구축하고, 이를 하나의 시스템으로 통합하는 대규모 협업 프로세스를 목표**
- **유기적인 시스템 통합 및 데이터 공유**: 중앙 관리(Manage) 팀에서 설계한 중앙 관리 시스템 및 데이터를 각 팀의 게임 특성에 맞춰 유기적으로 연동하고 활용하는 능력 향상
- **기술적 숙련도 및 디자인 패턴 적용**: 학습한 디자인 패턴(Singleton, Strategy(전략), State(상태) 패턴 등)을 실무 코드에 직접 적용하여 유지보수가 용이한 구조를 설계
- **역량 강화**: 실제 게임 엔진 환경에서 구현하며 개인의 개발 도메인 지식과 문제 해결 역량 강화


## 🛠 기술 경험 (Tech Experience)

### 1. 실무형 GitHub 협업 프로세스 (Fork & Pull Request)
**충돌 없는 병합을 위한 Fork & Smart Merge 워크플로우 구축**

- **Fork 기반의 안정적 독립 개발**: 메인 리포지토리의 안정성을 보장하기 위해 모든 작업을 개인 저장소로 Fork하여 진행했으며, 기능 단위 개발 후 검토를 거쳐 통합하는 체계적인 프로세스를 준수
- **Rebase & PR 시스템 실무 적용**: 작업 중간중간 메인 서버의 최신 코드를 Rebase로 반영하여 내 작업본을 최신화했으며, Manage 팀의 최종 승인을 받아야만 Main에 병합되는 '실무형 권한 관리 및 코드 리뷰' 절차를 경험
- **Unity 전용 Smart Merge 툴 활용**: Unity Smart Merge를 연동하여 13명의 작업물 사이에서 발생하는 데이터 크래시를 최소화
- **프로젝트 무결성 유지**: 많은 인원이 동시에 파일을 수정하는 환경에서도 Rebase -> Smart Merge -> PR 승인으로 이어지는 엄격한 워크플로우를 지킴으로써, 프로젝트 전체의 빌드가 깨지지 않도록 관리

&nbsp; &nbsp; 


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
