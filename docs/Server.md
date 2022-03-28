# Server

The server is running using a `HostedService` in ASP.NET Runtime. As long as there are running battles, this service will execute at 10 FPS and run the logic needed for the game.

When there isn't a battle raging, the timer is set to a long timeout and when a battle starts, it's cranked up to the full awesome 10 FPS again.

> The 10 FPS is of course a setting

One important point is that calls made to `DoWork` doesn't wait for the previous call to finish. So we need to see if we have an ongoing call or not. If we do, a frame drop is reported, indicating that you need a bigger server.

## Resources

[Microsoft Docs - Background Tasks with hosted services](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-6.0&tabs=visual-studio#timed-background-tasks)
