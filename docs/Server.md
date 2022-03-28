# Server

The server is running using a HostedService. As long as there are running battles, this service will execute at 10 FPS and run the logic needed for the game.

One important point is that calls made to DoWork doesn't wait for the previous call to finish. So we need to see if we have an ongoing call or not. If we do, a frame drop is reported, indicating that you need a bigger server.

## Resources

[Microsoft Docs - Background Tasks with hosted services](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-6.0&tabs=visual-studio#timed-background-tasks)
