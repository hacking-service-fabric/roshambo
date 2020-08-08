# Architecture

```
digraph Roshambo {
  start -> webhook -> start
  webhook -> engine
  webhook -> actor

  start [shape=plain label="Twilio"];
  
  webhook [label = "Roshambo.Twilio"]
  actor [label = "Roshambo.PlayerActor"]
  engine [label = "Roshambo.GameEngine"]
}
```