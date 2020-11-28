![Build status](https://dev.azure.com/hacking-service-fabric/roshambo/_apis/build/status/roshambo-Azure%20Service%20Fabric%20application-CI)](https://dev.azure.com/hacking-service-fabric/roshambo/_build/latest?definitionId=1)

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