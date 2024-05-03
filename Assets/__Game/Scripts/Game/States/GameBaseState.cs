using Assets.__Game.Scripts.Infrastructure;
using Assets.__Game.Scripts.StateMachine;

namespace Assets.__Game.Scripts.Game.States
{
  public abstract class GameBaseState : State
  {
    protected GameBootstrapper GameBootstrapper;
    protected Infrastructure.StateMachine StateMachine;
    protected SceneLoader SceneLoader;

    protected GameBaseState(GameBootstrapper gameBootstrapper)
    {
      GameBootstrapper = gameBootstrapper;
      StateMachine = GameBootstrapper.StateMachine;
      SceneLoader = GameBootstrapper.SceneLoader;
    }
  }
}