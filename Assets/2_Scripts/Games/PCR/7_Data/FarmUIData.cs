
namespace LUP.PCR
{
    public struct FarmUIData
    {
        public int level;
        public string buildingName;
        public int productionTime;
        public int curStorage;
        public int maxStorage;
        public int power;
        public bool isWorkRequested;

        public void SetData(int level, string buildingName, int productionTime, int curStorage, int maxStorage, int power, bool isWorkRequested)
        {
            this.level = level;
            this.buildingName = buildingName;
            this.curStorage = curStorage;
            this.maxStorage = maxStorage;
            this.power = power;
            this.isWorkRequested = isWorkRequested;
        }
    }
}

