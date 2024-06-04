using ApplesToApples.Cards;
using ApplesToApples.Players;
using ApplesToApples.Resources;
using ApplesToApples.States;
using ApplesToApples.Utilities;

namespace ApplesToApples.Builders;

public class StandardCreator : IGameCreator
{
    List<GreenApple> _greenApples = new List<GreenApple>();
    List<IRedApple> _redApples = new List<IRedApple>();
    List<IGameState> _states = new List<IGameState>();
    
    public Apples2Apples Create()
    {
        Apples2Apples game = new Apples2Apples(); // Replace with dependency injection
        
        // Read all the green apples (adjectives) from a file and add to the green apples deck
        foreach (string adjective in Resource.GetGreenApples().Split('\n'))
        {
            _greenApples.Add(new GreenApple(adjective));
        }
        
        // Read all the red apples (nouns) from a file and add to the red apples deck
        foreach (string noun in Resource.GetRedApples().Split('\n'))
        {
            _redApples.Add(new RedApple(noun));
        }
        
        // Shuffle both the green apples and red apples decks
        _greenApples.Shuffle();
        _redApples.Shuffle();
        
        // Deal seven red apples to each player, including the judge
        foreach (PlayerPawn player in game.GetPlayers())
        {
            for (int i = 0; i < 7; i++)
            {
                player.GiveRedApple(_redApples.Pop(0));
            }
        }

        DrawState<GreenApple> drawState = new DrawState<GreenApple>(_greenApples.GetEnumerator());
        SubmitState submitState = new SubmitState(game.GetControllers, drawState.GetCurrent);
        JudgeState<IRedApple> judgeState = new JudgeState<IRedApple>(submitState.GetRedApples, new DictatorSelector<IRedApple>()); 
        
        //_states.Add(new DrawState<GreenApple>(_greenApples.GetEnumerator(), ));
        //_states.Add(new SubmitState(game.GetPlayers, ));
        
        // STATE MACHINE, LET STATES KNOW ABOUT EACHOTHER TO SIMPLIFY THIS MESS, SHOULD BE ABLE TO SIMULATE GAME
        // ADD PROXY? TO PRINT WHAT'S HAPPENING TO USERS OR JUST SIMULATE WITH BOTS AND REAL GAMES WRITE IN PLAYER CONTROLLER
        
        // HAVE PLAYERCONTROLLER TAKE PLAYERPAWN AS ARG THEN U CAN REPLACE HUMAN PLAYER WITH BOT IF THEY LEAVE!
        
        return game;
    }
    
}