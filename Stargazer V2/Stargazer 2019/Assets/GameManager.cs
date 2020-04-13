

public class GameManager
{
    public static bool gameEnded = false;
    public static int lastLevelDeath = 0;

    public static string GetObjective(int buildIndex)
    {
        switch (buildIndex)
        {
            case 1:
                return "Objective: Kill all enemies";
            case 2:
                return "Objective: Kill all enemies";
            case 3:
                return "Objective: Kill all enemies";
            case 4:
                return "Objective: Kill all enemies";
            case 5:
                return "Objective: Get to the end";
            case 6:
                return "Objective: Get to the end";
            case 7:
                return "Objective: Beat the Boss and Press the Button";
            default:
                return "";
        }
    }
}
