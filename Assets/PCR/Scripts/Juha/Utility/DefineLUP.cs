using UnityEngine;

// 테스트용 사라질 예정
public static class GridSize
{
    public static int x = 10;
    public static int y = 10;
}

public static class BBKeys
{
    public const string Self = "Self";         // Worker 컴포넌트
    public const string OwnerAI = "OwnerAI";   // WorkerAI 컴포넌트
    public const string Hunger = "Hunger";
    public const string IsWorking = "IsWorking";
    public const string HasNewTask = "HasNewTask";
    public const string HasPausedTask = "HasPausedTask";
}

public enum TileType
{
    NONE,
    PATH,
    WALL,
    BUILDING,
}

public enum BuildingType
{
    NONE,
    MUSHROOMFARM,
    WHEATFARM,
    MOLEFARM,
    RESTAURANT
}

public enum WallType
{
    NONE,
    DUST,
    STONE
}

public enum ResourceType
{
    STONE,
    COAL,
    IRON,
    WHEAT,
    MUSHROOM,
}

public enum PlacementResultType
{
    SUCCESS,
    NOTENOUGHSPACE,
    LACKOFRESOURCE // 자원 종류별로 하나씩
}

public enum BuildState
{
    UNDERCONSTRUCTION,
    COMPLETED
}

public enum CropType
{
    NONE,
    WHEAT,
    POTATO
}

public enum FoodType
{
    NONE,
    BREAD,
    POTATOPIZZA
}

public enum TaskType
{
    BuildingWheatFarm,
    BuildingMushroomFarm,
}