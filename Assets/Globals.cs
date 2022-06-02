public class Globals {
  public static BuildingData[] BUILDING_DATA = new BuildingData[] {
    new BuildingData("Building", 100)
  };
  public static int TERRAIN_LAYER_MASK = 1 << 6;
  public static int PROJECTILE_LAYER_MASK = 1 << 7;
  public static int CAMERA_FRUSTUM_COLLIDER_LAYER_MASK = 1 << 8;

  public static int CAMERA_FRUSTUM_COLLIDER_LAYER_INDEX = 8;
  public static int PROJECTILE_LAYER_INDEX = 7;
  public static UnitData[] UNIT_DATA = new UnitData[] {
    new UnitData("Unit")
  };

  public static ArrowPool ArrowPool = new ArrowPool(200, 100);
}
