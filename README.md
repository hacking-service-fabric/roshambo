Here's a cool image...

![Architecture](https://quickchart.io/graphviz?graph=digraph Roshambo {
  start -> receiver -> queue -> actor -> engine -> responder -> end

  start [shape=plain label="Request"];
  end [shape=plain label="End"];
  
  receiver [label = "Roshambo.Twilio.Receiver"]
  queue [label = "Roshambo.Queue"]
  actor [label = "Roshambo.PlayerActor"]
  engine [label = "Roshambo.GameEngine"]
  responder [label = "Roshambo.Twilio.Responder"]
})

Did it work?