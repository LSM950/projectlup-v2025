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

**Fork 기반의 안정적 독립 개발**: 메인 리포지토리의 안정성을 보장하기 위해 모든 작업을 개인 저장소로 Fork하여 진행했으며, 기능 단위 개발 후 검토를 거쳐 통합하는 체계적인 프로세스를 준수

**Rebase & PR 시스템 실무 적용**: 작업 중간중간 메인 서버의 최신 코드를 Rebase로 반영하여 내 작업본을 최신화했으며, Manage 팀의 최종 승인을 받아야만 Main에 병합되는 '실무형 권한 관리 및 코드 리뷰' 절차를 경험

**Unity 전용 Smart Merge 툴 활용**: Unity Smart Merge를 연동하여 13명의 작업물 사이에서 발생하는 데이터 크래시를 최소화

**프로젝트 무결성 유지**: 많은 인원이 동시에 파일을 수정하는 환경에서도 Rebase -> Smart Merge -> PR 승인으로 이어지는 엄격한 워크플로우를 지킴으로써, 프로젝트 전체의 빌드가 깨지지 않도록 관리

&nbsp; &nbsp; 

### 2. 행동 트리(Behavior Tree) 기반의 모듈형 AI 시스템 자체 구축
**행동 트리 구조를 코드 레벨에서 직접 설계하고 구현**

BaseNode를 상속받는 Selector, Sequence, ActionNode, ConditionNode 등의 기본 노드를 구현하여 조건과 행동을 트리 형태로 조합할 수 있는 기반을 마련
<details>
<summary> Behavior Tree </summary>

```cs

```
</details>

  
플레이어의 조작 상태(수동/자동)와 탄약, 적 감지 여부에 따라 분기되는 복잡한 판단 로직을 CharacterBT 클래스에 시각적이고 직관적인 트리 구조로 매핑하여 AI의 유지보수성을 극대화
<details>
<summary> CharacterBT 예시 </summary>

```cs
//대표 예시 : CharacterBT_Range
  public class CharacterBT_Range : BehaviorTreeBase
  {
      private RangeBlackBoard character;
      private RangeActions characterActions;

      protected override BaseNode SetupTree()
      {
          character = GetComponent<RangeBlackBoard>();
          characterActions = GetComponent<RangeActions>();

          NodeState Retire() => characterActions.Retire(character);
          NodeState FireManual() => characterActions.FireManual(character);
          NodeState FireAuto() => characterActions.FireAuto(character);
          NodeState Cover() => characterActions.Cover(character);
          NodeState Reload() => characterActions.Reload(character);

          // 수동 모드 행동트리
          Selector manualSelector = new Selector(new List<BaseNode>
          {
              // 1. 플레이어 입력 + 탄약 + 재장전 중 아님 → 발사
              new Sequence(new List<BaseNode>
              {
                  new ConditionNode(() => character.IsPlayerInputExists()),
                  new ConditionNode(() => character.HasAmmo()),
                  new ConditionNode(() => !characterActions.IsReloading),
                  new ActionNode(FireManual)
              }),

              // 2. 재장전 중이면 유지
              new Sequence(new List<BaseNode>
              {
                  new ConditionNode(() => characterActions.IsReloading),
                  new ActionNode(Reload)
              }),

              // 3. 탄약 없으면 재장전 시작
              new Sequence(new List<BaseNode>
              {
                  new ConditionNode(() => !character.HasAmmo()),
                  new ActionNode(Reload)
              }),

              // 4. 기본 대기
              new ActionNode(Cover)
          });

          // 자동 모드 행동트리 
          Selector autoSelector = new Selector(new List<BaseNode>
          {
              // 1. 재장전 중이면 재장전 완료까지 대기
              new Sequence(new List<BaseNode>
              {
                  new ConditionNode(() => characterActions.IsReloading),
                  new ActionNode(Reload)
              }),
          
              // 2. 적 있고 탄약 있으면 자동 공격
              new Sequence(new List<BaseNode>
              {
                  new ConditionNode(() => character.IsEnemyInRange()),
                  new ConditionNode(() => character.HasAmmo()),
                  new ConditionNode(() => !characterActions.IsReloading),
                  new ActionNode(FireAuto)
              }),
          
              // 3. 기본 대기 
              new ActionNode(Cover)
          });

          // 메인 행동트리
          return new Selector(new List<BaseNode>
          {
              // HP 0 이하면 리타이어
              new Decorator(character.IsHpZero, new ActionNode(Retire)),
          
              // 모드에 따른 분기
              new Selector(new List<BaseNode>
              {
                  new Decorator(character.IsManualMode, manualSelector),
                  autoSelector
              })
          });
      }
  }
// 이외 구현체: CharacterBT_Melee
```
</details>

&nbsp; &nbsp; 

### 3. 제네릭 오브젝트 풀(Generic Object Pool)과 웨이브 스폰 시스템 설계
**수많은 몬스터와 투사체가 등장하는 디펜스 장르의 병목 현상을 해결하고, 스테이지를 자유롭게 구성할 수 있는 스폰 시스템을 개발**
IPoolable 인터페이스와 제네릭(<T>)을 활용한 범용 ObjectPool을 구현하여 몬스터와 투사체의 생성/파괴(Instantiate/Destroy)에 따른 가비지 컬렉션(GC) 부하를 제거
<details>
<summary> Behavior Tree </summary>

```cs

```
</details>

MonsterSpawner를 통해 순차 스폰 및 랜덤 스폰 방식을 지원하는 WaveData 구조를 설계하고, 출전한 팀의 평균 레벨에 비례하여 몬스터의 스탯이 오르는 동적 난이도 조절(Dynamic Difficulty Scaling) 로직을 적용
<details>
<summary> Behavior Tree </summary>

```cs

```
</details>



&nbsp; &nbsp; 

### 4.ScriptableObject와 JSON을 결합한 데이터 파이프라인 구축
**대규모 협업 환경에서 데이터의 무결성을 유지하고, 캐릭터 정보 및 게임 진행 상태를 효율적으로 관리하는 구조를 설계**

불변하는 고정 데이터(이름, 프리팹, 썸네일 등)는 **ScriptableObject (STCharacterData)** 로 관리하여 메모리를 최적화
<details>
<summary> STCharacterData </summary>

```cs

```
</details>

변동하는 런타임 데이터(레벨, 경험치, 팀 배치)는 **JSON 직렬화(ShootingRuntimeData)** 를 통해 저장하고 불러오도록 분리하여, Manage 팀의 중앙 데이터 규격과 유연하게 연동되는 세이브/로드 시스템을 구현했습니다.
<details>
<summary> ShootingRuntimeData </summary>

```cs

```
</details>

&nbsp; &nbsp; 


## 🛠 핵심 개발 상세 (Technical Deep Dive)

### 1. 수동/자동 전환이 가능한 하이브리드 전투 시스템
**블랙보드 패턴 적용**: RangeBlackBoard를 통해 캐릭터의 탄약, 현재 모드, 적 감지 상태 등의 데이터를 한곳에서 관리하여 노드 간 결합도를 낮췄습니다.
