# Project-LUP
## 인하대학교 미래인재개발원 Unity 팀 프로젝트

<img width="817" height="461" alt="image" src="https://github.com/user-attachments/assets/36729c49-7b03-4370-be11-909e7f076a3a" />


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

&nbsp; &nbsp; 

## 🛠 기술 경험 (Tech Experience)

### 1. 실무형 GitHub 협업 프로세스 (Fork & Pull Request)
**충돌 없는 병합을 위한 Fork & Smart Merge 워크플로우 구축**

**Fork 기반의 안정적 독립 개발**: 메인 리포지토리의 안정성을 보장하기 위해 모든 작업을 개인 저장소로 Fork하여 진행했으며, 기능 단위 개발 후 검토를 거쳐 통합하는 체계적인 프로세스를 준수

**Rebase & PR 시스템 실무 적용**: 작업 중간중간 메인 서버의 최신 코드를 Rebase로 반영하여 내 작업본을 최신화했으며, Manage 팀의 최종 승인을 받아야만 Main에 병합되는 '실무형 권한 관리 및 코드 리뷰' 절차를 경험

**Unity 전용 Smart Merge 툴 활용**: Unity Smart Merge를 연동하여 13명의 작업물 사이에서 발생하는 데이터 크래시를 최소화

**프로젝트 무결성 유지**: 많은 인원이 동시에 파일을 수정하는 환경에서도 Rebase -> Smart Merge -> PR 승인으로 이어지는 엄격한 워크플로우를 지킴으로써, 프로젝트 전체의 빌드가 깨지지 않도록 관리

<img width="1920" height="1043" alt="image" src="https://github.com/user-attachments/assets/bdad2702-c123-4556-8493-a3622a970caf" />


&nbsp; &nbsp; 

### 2. 행동 트리(Behavior Tree) 기반의 모듈형 AI 시스템 자체 구축

기존 상태 머신(FSM)이 가지는 확장성의 한계를 극복하기 위해, 상용 에셋에 의존하지 않고 행동 트리 구조를 코드 레벨에서 직접 설계하고 구현

* **코어 시스템 설계**: `BaseNode`를 상속받는 `Selector`, `Sequence`, `ActionNode`, `ConditionNode` 등의 기본 노드를 구현하여 조건과 행동을 트리 형태로 조합할 수 있는 기반을 마련

    <details>
    <summary> 💻 Behavior Tree 기반 구조 (Base / Composite / Leaf) 구현체 </summary>
    
    ```cs
    public abstract class BaseNode
    {
        protected NodeState state;
        public abstract NodeState Evaluate();
    }
    
    public class Selector : BaseNode
    {
        public Selector(List<BaseNode> children) { this.children = children; }
        public override NodeState Evaluate()
        {
            foreach (BaseNode node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE: continue;
                    case NodeState.SUCCESS: return NodeState.SUCCESS;
                    case NodeState.RUNNING: return NodeState.RUNNING;
                }
            }
            return NodeState.FAILURE;
        }
    }
    
    public class ActionNode : BaseNode
    {
        private Func<NodeState> action;
        public ActionNode(Func<NodeState> action) { this.action = action; }
        public override NodeState Evaluate() => action();
    }
    ```
    </details>

* **직관적인 AI 로직 매핑**: 플레이어의 조작 상태(수동/자동), 탄약 잔량, 적 감지 여부에 따라 분기되는 복잡한 판단 로직을 CharacterBT 클래스에 시각적이고 직관적인 트리 구조로 매핑하여 유지보수성을 극대화
    <details>
    <summary> 💻 CharacterBT_Range (수동/자동 하이브리드 전투 AI 설계) </summary>
    
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

**수많은 몬스터와 투사체가 등장하는 슈팅 디펜스 장르 특성상, 잦은 인스턴스 생성 및 파괴(Instantiate/Destroy)로 인한 가비지 컬렉션(GC) 스파이크를 방지**

* IPoolable 인터페이스와 제네릭(<T>)을 활용하여 어떤 컴포넌트든 재사용할 수 있는 범용 ObjectPool 클래스를 구현

* 큐(Queue) 자료구조를 활용해 비활성화된 객체를 관리하고, 풀이 고갈되었을 때만 동적으로 추가 생성하도록 설계하여 메모리 사용량을 안정적으로 유지

    
    <details>
    <summary> 💻 ObjectPool 핵심 할당 및 반환 로직 </summary>

    ```cs
     public class ObjectPool<T> where T : Component
     {
         private T prefab;
         private Transform poolParent;
         private Queue<T> availableObjects = new Queue<T>();
         private List<T> allObjects = new List<T>();
    
         public ObjectPool(T prefab, int initialSize, Transform parent = null)
         {
             this.prefab = prefab;
             GameObject poolObject = new GameObject($"{prefab.name}_Pool");
             poolParent = poolObject.transform;
             if (parent != null)
                 poolParent.SetParent(parent);

             for (int i = 0; i < initialSize; i++)
             {
                 CreateNewObject();
             }
         }

         private T CreateNewObject()
         {
             T newObj = GameObject.Instantiate(prefab, poolParent);
             newObj.gameObject.SetActive(false);
             availableObjects.Enqueue(newObj);
             allObjects.Add(newObj);
             return newObj;
         }

         public T Get(Vector3 position, Quaternion rotation)
         {
             T obj;

             if (availableObjects.Count == 0)
             {
                 obj = CreateNewObject();
                 Debug.LogWarning($"Pool exhausted! Creating new {prefab.name}");
             }
             else
             {
                 obj = availableObjects.Dequeue();
             }

             obj.transform.position = position;
             obj.transform.rotation = rotation;
             obj.gameObject.SetActive(true);

             IPoolable poolable = obj.GetComponent<IPoolable>();
             poolable?.OnSpawn();
    
             return obj;
         }

         public void Return(T obj)
         {
             IPoolable poolable = obj.GetComponent<IPoolable>();
             poolable?.OnDespawn();
    
             obj.gameObject.SetActive(false);
             availableObjects.Enqueue(obj);
         }

         public void ReturnAll()
         {
             foreach (T obj in allObjects)
             {
                 if (obj.gameObject.activeSelf)
                 {
                     Return(obj);
                 }
             }
         }

         public int TotalCount => allObjects.Count;
         public int ActiveCount => allObjects.Count - availableObjects.Count;
         public int AvailableCount => availableObjects.Count;
     }
    
    ```
    </details>

* MonsterSpawner를 통해 순차 스폰 및 랜덤 스폰 방식을 지원하는 WaveData 구조를 설계하고, 출전한 팀의 평균 레벨에 비례하여 몬스터의 스탯이 오르는 동적 난이도 조절(Dynamic Difficulty Scaling) 로직을 적용


  <details>
  <summary> 💻 MonsterSpawner (동적 난이도 및 Wave 스폰 로직) </summary>

  ```cs
  // 1. 동적 난이도 조절 (출전 팀 평균 레벨 기반)
  private float CalculateDifficultyMultiplier()
  {
      int totalLevel = 0;
      var srd = STDataManage.Instance?.RuntimeData;
      
      if (srd != null && srd.Team != null)
      {
          foreach (var charData in srd.Team)
          {
              if (charData != null)
                  totalLevel += srd.GetCharacterLevel(charData.characterId);
          }
      }
  
      if (totalLevel < 2) totalLevel = 1;
      // 팀 레벨 총합에 비례하여 몬스터 스탯 배율 증가 (레벨당 10%)
      return 1f + (totalLevel * 0.1f); 
  }
  
  // 스폰 시 몬스터에게 난이도 배율 적용
  private void SetMonsterStats(MonsterData monster)
  {
      var stats = monster.GetComponent<StatComponent>();
      if (stats != null)
      {
          // 계산된 배율을 몬스터 체력, 공격력 등에 곱연산 적용
          stats.ScaleStats(difficultyMultiplier);
      }
  }
  
  // 2. WaveData를 활용한 스폰 코루틴 (순차/랜덤 스폰 지원)
  private IEnumerator SpawnWave(WaveData wave)
  {
      isSpawning = true;
      if (wave.useRandomSpawn)
      {
          // 랜덤 스폰 모드
          for (int i = 0; i < wave.randomSpawnCount; i++)
          {
              SpawnMonster(wave.GetRandomMonster());
              yield return new WaitForSeconds(wave.spawnInterval);
          }
      }
      else
      {
          // 순차 스폰 모드 (정해진 몬스터 배열에 따라 스폰)
          foreach (var (prefab, delay) in wave.GetSpawnSequence())
          {
              if (delay > 0) yield return new WaitForSeconds(delay);
              SpawnMonster(prefab);
          }
      }
      isSpawning = false;
  }

  ```

  </details>

&nbsp; &nbsp; 


### 4.ScriptableObject와 JSON을 결합한 데이터 파이프라인 구축
**대규모 협업 환경에서 데이터의 무결성을 유지하고, 캐릭터 정보 및 게임 진행 상태를 효율적으로 관리하는 구조를 설계**

* 불변하는 고정 데이터(이름, 프리팹, 썸네일 등)는 **ScriptableObject (STCharacterData)** 로 관리하여 메모리를 최적화
    <details>
    <summary> 💻 STCharacterData (자산 데이터 정의)  </summary>
    
    ```cs

    [CreateAssetMenu(fileName = "CharacterData", menuName = "ST/CharacterData")]
    public class STCharacterData : ScriptableObject
    {
        public int characterId;      // 중앙 관리용 고유 ID
        public string charName;      // 캐릭터 이름
        public Sprite thumbnail;     // UI 썸네일 리소스
        public GameObject prefab;    // 실제 소환될 프리팹
        
        public float baseHp;         // 기본 스탯 정보
        public float baseAtk;
    }

    ```
    </details>

* 변동하는 런타임 데이터(레벨, 경험치, 팀 배치)는 **JSON 직렬화(ShootingRuntimeData)** 를 통해 저장하고 불러오도록 분리하여, Manage 팀의 중앙 데이터 규격과 유연하게 연동되는 세이브/로드 시스템을 구현
      
  <details>
  <summary> 💻 ShootingRuntimeData (JSON 직렬화 및 데이터 관리)</summary>
  
  ```cs
  [System.Serializable]
  public class ShootingRuntimeData
  {
      // 유저의 캐릭터 성장 데이터 및 팀 구성 정보
      public List<CharacterRuntimeStatus> ownedCharacters = new List<CharacterRuntimeStatus>();
      public int[] currentTeam = new int[5]; 
  
      // 중앙 데이터 규격에 맞춘 JSON 저장 로직
      public void SaveData()
      {
          string json = JsonUtility.ToJson(this);
          // 중앙 관리 시스템(Manage팀)의 데이터 경로 혹은 PlayerPrefs에 저장
          File.WriteAllText(Application.persistentDataPath + "/SaveData.json", json);
      }
  
      // 데이터 로드 및 ScriptableObject 데이터와 동기화
      public void LoadData()
      {
          string path = Application.persistentDataPath + "/SaveData.json";
          if (File.Exists(path))
          {
              string json = File.ReadAllText(path);
              JsonUtility.FromJsonOverwrite(json, this);
          }
      }
  }
  ```
  </details>

&nbsp; 

&nbsp; 

&nbsp; 

&nbsp; 

## 🛠 핵심 개발 상세 (Technical Deep Dive)
<img width="696" height="393" alt="image" src="https://github.com/user-attachments/assets/ebfc19de-e876-4fba-b850-fc39d8006826" />
<img width="697" height="397" alt="image" src="https://github.com/user-attachments/assets/e695599d-f68c-46bf-922b-8fb011d13aa0" />

### 1. 수동/자동 전환이 가능한 하이브리드 전투 시스템

**블랙보드 패턴 적용**: AI가 행동을 결정할 때마다 매번 컴포넌트를 탐색(GetComponent)하거나 상태를 계산하는 연산 낭비를 막기 위해 RangeBlackBoard를 도입

데이터를 한곳으로 중앙화하여 모듈화된 트리 노드들이 블랙보드만 참조하게 함으로써, 새로운 캐릭터나 조건이 추가되어도 기존 AI 로직을 수정할 필요가 없는 높은 확장성을 확보

<details>
<summary> 💻 RangeBlackBoard (상태 데이터 중앙화) </summary>

```cs
public class RangeBlackBoard : MonoBehaviour
{
    public bool isManualMode = false; // 현재 조작 여부
    public int currentAmmo = 20;
    public float detectionRange = 15f;
    public Transform currentTarget;

    // AI 노드가 판단 근거로 사용할 헬퍼 메서드
    public bool HasAmmo() => currentAmmo > 0;
    public bool IsEnemyInRange() => currentTarget != null && 
        Vector2.Distance(transform.position, currentTarget.position) <= detectionRange;
}
```
</details>

&nbsp;

**동적 AI 전환 & 스마트 타겟팅**: 수동 조작 시에는 플레이어의 입력이 최우선이 되고, 자동 모드일 때는 CombatUtility와 거리를 계산하여 독자적으로 판단

하나의 컨트롤러가 모든 캐릭터를 통제하는 스파게티 코드를 피하고, 각 캐릭터가 자신의 트리를 독립적으로 평가하는 객체 지향적 설계를 구현

<details>
<summary> 💻 모드 전환 및 BT 분기 로직 </summary>

```cs
// 캐릭터 선택 시 모드 스위칭
public void SwitchControl(int targetIndex)
{
    for (int i = 0; i < allUnits.Count; i++)
    {
        allUnits[i].BlackBoard.isManualMode = (i == targetIndex);
    }
}

// Behavior Tree 내의 모드 분기 설계
protected override BaseNode SetupTree()
{
    return new Selector(new List<BaseNode>
    {
        // 수동 모드 조건 만족 시 플레이어 컨트롤 트리 실행
        new Decorator(() => blackboard.isManualMode, manualCombatTree),
        // 수동이 아닐 경우 자동 전투 트리 실행
        autoCombatTree 
    });
}
```
</details>

<details>
<summary> 💻 조준 및 발사 로직 (CombatUtility)  </summary>

```cs
public static class CombatUtility
{
    public static void AimAndShoot(Transform firePoint, Transform target, float damage)
    {
        if (target == null) return;

        // 적 위치로 부드러운 회전 및 발사 방향 설정
        Vector3 dir = (target.position - firePoint.position).normalized;
        firePoint.rotation = Quaternion.LookRotation(dir);

        // ObjectPool에서 투사체 소환
        var projectile = ProjectilePool.Instance.Get(firePoint.position, firePoint.rotation);
        projectile.GetComponent<Projectile>().Setup(damage);
    }
}
```
</details>

&nbsp; &nbsp; 

### 2. 다이나믹 시점 전환 및 터치/마우스 조준 카메라

**수학적 보간을 활용한 시점 제어 (CameraController)**: 카메라 위치를 순간 이동(Snap)시키는 방식을 사용했을시 어색한 부분이 존재

이를 해결하기 위해 전장을 넓게 보는 오버뷰(Overview)와 캐릭터 등 뒤를 비추는 숄더뷰(Focus) 모드 간의 이동을 Vector3.Lerp와 Quaternion.Slerp로 구현하여, **항상 부드럽고 일관된 카메라 트랜지션(Transition)**을 구현

<details>
<summary> 💻 Camera SwitchView 로직 </summary>

```cs
public void SwitchView(CameraMode mode, Transform target = null)
{
    Vector3 targetPos = (mode == CameraMode.Focus) ? target.position + offset : overviewPos;
    Quaternion targetRot = (mode == CameraMode.Focus) ? focusRotation : overviewRotation;

    // 코루틴이나 Update 내에서 부드러운 보간 이동
    transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * lerpSpeed);
    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * lerpSpeed);
}
```
</details>

&nbsp; &nbsp; 
**동적 FOV(시야각) 줌 시스템**: 저격 등 정밀 타격을 위한 StartZoom 호출 시, 카메라의 fieldOfView를 점진적으로 좁히며 자연스러운 줌 인(Zoom-In) 효과를 연출

<details>
<summary> 💻 Dynamic FOV Zoom 로직 </summary>

```cs
private IEnumerator Co_Zoom(float targetFOV)
{
    float currentFOV = mainCamera.fieldOfView;
    float elapsed = 0f;
    while (elapsed < zoomDuration)
    {
        elapsed += Time.deltaTime;
        mainCamera.fieldOfView = Mathf.Lerp(currentFOV, targetFOV, elapsed / zoomDuration);
        yield return null;
    }
}
```
</details>

&nbsp; &nbsp; 
**사용자 경험(UX)을 고려한 자유 조준 로직**:면 드래그 양(Input.GetAxis)을 기반으로 상하좌우 회전각(Pitch, Yaw)을 누적

이때 Mathf.Clamp를 통한 엄격한 회전 제한(Gimbal Lock 방지)을 적용하여, 긴박한 전투 상황에서도 시점이 튀거나 뒤집히지 않는 안정적인 슈팅 환경을 구축

<details>
<summary> 💻 Aim Rotation & Clamping 로직 </summary>

```cs
private void HandleRotation()
{
    yaw += Input.GetAxis("Mouse X") * sensitivity;
    pitch -= Input.GetAxis("Mouse Y") * sensitivity;

    // 상하 회전 각도 제한 (예: -30도 ~ 60도)
    pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

    targetRotation = Quaternion.Euler(pitch, yaw, 0);
    transform.rotation = targetRotation;
}
```
</details>

&nbsp; &nbsp; 

### 3. Dictionary 기반의 유연한 스탯 및 버프 관리

런타임 오버헤드 제거 및 이벤트 주도(Event-Driven) 설계로 확장성 있는 스탯 시스템 구축

**IDamageable 인터페이스**: 플레이어, 몬스터 등 체력을 가진 모든 객체가 TakeDamage 메서드를 공유하도록 설계하여 타격 판정 로직을 단일화
<details>
<summary> 💻 IDamageable 인터페이스 </summary>

```cs
public interface IDamageable
{
    void TakeDamage(float damage, Vector3 hitPoint);
    bool IsDead { get; }
}
```
</details>

&nbsp; &nbsp; 
**코루틴 & Dictionary 기반 O(1) 버프 스케줄링**: List 대신 Dictionary<string, Coroutine>을 사용하여 공격력, 방어력 등 다양한 버프 상태를 O(1)의 시간 복잡도로 빠르게 탐색하고 갱신하도록 설계

이를 통해 동일한 버프가 중복 적용되지 않고 지속 시간만 연장되도록 안전하고 최적화된 버프 시스템을 구현

<details>
<summary> 💻 Buff Management (Overwrite Logic) </summary>

```cs
private Dictionary<string, Coroutine> activeBuffs = new Dictionary<string, Coroutine>();

public void ApplyBuff(string buffId, float duration, Action onEnd)
{
    // 동일한 버프가 이미 실행 중이라면 중단 후 갱신(중첩 방지)
    if (activeBuffs.ContainsKey(buffId))
    {
        StopCoroutine(activeBuffs[buffId]);
    }
    activeBuffs[buffId] = StartCoroutine(Co_BuffTimer(buffId, duration, onEnd));
}
```
</details>

&nbsp; &nbsp; 
**Update() 오버헤드를 제거한 Event-Driven UI**: 수의 몬스터가 등장하는 디펜스 장르 특성상 매 프레임 UI를 갱신하면 병목이 발생

체력이 변경되거나 사망할 때 OnHealthChanged, OnDeath Action 이벤트를 발생시켜, 체력바와 데미지 팝업이 상태가 변하는 순간에만 이벤트 주도적(Event-Driven)으로 반응하도록 최적화

<details>
<summary> 💻 Event-Driven UI Update </summary>

```cs
public class StatComponent : MonoBehaviour
{
    public event Action<float, float> OnHealthChanged; // (current, max)
    public event Action OnDeath;

    public void DecreaseHp(float amount)
    {
        currentHp = Mathf.Max(0, currentHp - amount);
        OnHealthChanged?.Invoke(currentHp, maxHp); // UI는 이 시점에만 갱신됨

        if (currentHp <= 0) OnDeath?.Invoke();
    }
}
```
</details>

&nbsp; &nbsp; 
