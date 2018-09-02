using DG.Tweening;
using GiraffeStar;

public class RaceBootstrap
{
    private static bool isInitialized;

    public RaceBootstrap()
    {
        if(isInitialized) { return; }

        Init();

        isInitialized = true;
    }

    void Init()
    {
        Config.Init();
        GiraffeSystem.Init();

        InitPlugins();
        InitModules();
    }

    void InitPlugins()
    {
        DOTween.Init().SetCapacity(200, 10);
    }

    void InitModules()
    {
        GiraffeSystem.Register(new UIModule());
        GiraffeSystem.Register(new RaceModule());
        GiraffeSystem.Register(new TutorialModule());
        GiraffeSystem.Register(new CreditModule());
    }
}
