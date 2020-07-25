# Architecture

```
digraph Roshambo {
  start -> receiver -> queue -> actor -> engine -> responder -> end

  start [shape=plain label="Request"];
  end [shape=plain label="End"];
  
  receiver [label = "Roshambo.Twilio.Receiver"]
  queue [label = "Roshambo.Queue"]
  actor [label = "Roshambo.PlayerActor"]
  engine [label = "Roshambo.GameEngine"]
  responder [label = "Roshambo.Twilio.Responder"]
}
```