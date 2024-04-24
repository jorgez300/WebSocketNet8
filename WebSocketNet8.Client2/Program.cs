using WebSocketNet8Library;

Console.WriteLine("Press any key to start");
Console.ReadLine();



SocketClientService ws = new SocketClientService();

await ws.connect("Client 2");


_ = Task.Run(async () =>
{
    while (true)
    {
        Console.WriteLine("Write a message");
        string msg = Console.ReadLine();
        await ws.Send(msg);

    }

}, ws.cts.Token);

_ = Task.Run(async () =>
{
    while (true)
    {

        string msg = await ws.Receive();
        await Console.Out.WriteLineAsync(msg);

    }

}, ws.cts.Token);


while (ws.isConnected())
{

}




Console.WriteLine("Press any key to end");
Console.ReadLine();









