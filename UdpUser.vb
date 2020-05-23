Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Public Structure Received
    Public Sender As IPEndPoint
    Public Message As String
End Structure

MustInherit Class UdpBase
    Protected Client As UdpClient

    Protected Sub New()
        Client = New UdpClient()
    End Sub

    Public Async Function Receive() As Task(Of Received)
        Dim result = Await Client.ReceiveAsync()
        Return New Received() With {
            .Message = Encoding.ASCII.GetString(result.Buffer, 0, result.Buffer.Length),
            .Sender = result.RemoteEndPoint
        }
    End Function
End Class

Class UdpListener
    Inherits UdpBase

    Private _listenOn As IPEndPoint

    Public Sub New()
        Me.New(New IPEndPoint(IPAddress.Any, 32123))
    End Sub

    Public Sub New(ByVal endpoint As IPEndPoint)
        _listenOn = endpoint
        Client = New UdpClient(_listenOn)
    End Sub

    Public Sub Reply(ByVal message As String, ByVal endpoint As IPEndPoint)
        Dim datagram = Encoding.ASCII.GetBytes(message)
        Client.Send(datagram, datagram.Length, endpoint)
    End Sub
End Class

Class UdpUser
    Inherits UdpBase

    Private Sub New()
    End Sub

    Public Shared Function ConnectTo(ByVal hostname As String, ByVal port As Integer) As UdpUser
        Dim connection = New UdpUser()
        connection.Client.Connect(hostname, port)
        Return connection
    End Function

    Public Sub Send(ByVal message As String)
        Dim datagram = Encoding.ASCII.GetBytes(message)
        Client.Send(datagram, datagram.Length)
    End Sub
End Class
