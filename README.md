# c-vb.net-syslog-client
C# or Vb.net Syslog client example. Send syslog messages.

Usage : 

    
    Dim client = UdpUser.ConnectTo("127.0.0.1", 514)
    client.Send("test message")
