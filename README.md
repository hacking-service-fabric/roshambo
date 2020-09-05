# Architecture

```
digraph Roshambo {
  start -> webhook -> start
  webhook -> engine
  webhook -> actor
  actor -> engine

  start [shape=plain label="Twilio"];
  
  webhook [label = "Roshambo.Twilio \n ITranslationService"]
  actor [label = "Roshambo.PlayerSessionActor \n IPlayerSession"]
  engine [label = "Roshambo.GameServices \n IGameService \n IGameOptionService"]
}
```