# HotwireTurbo for ASP.NET Core
Use [Hotwire Turbo](https://turbo.hotwire.dev/) in your ASP.NET Core app.

## This NuGet package provides
* Tag helpers for `<turbo-frame>` and `<turbo-stream>`
* Extension methods
  * `request.IsTurboStreamRequest()`
  * `request.IsTurboFrameRequest()`
* An action result that allows you to reuse your existing partials
  * Wraps your partial in a `<turbo-stream>` element
  * `return TurboStreamResult("replace", message)`
* A `IHasDomId` interface to match the [rails turbo api](https://github.com/hotwired/turbo-rails)
  * Add this to your models to provide an elegant way to set the id and target attributes
  * `public string ToDomId() => $"message_{Id}";` => `<turbo-frame id="message_1">"`

## Installation
* Install the NuGet package 
* In `Startup.cs` add `services.AddHotwireTurbo();`
* Add helpers to your `BaseController` class that call `TurboStreamResult(...)`

```
public TurboStreamResult TurboStreamResult(string action, IHasDomId target, string viewName = null, object model = null) {
    return TurboStreamResult(action, target.ToDomId(), viewName, model);
}        

public  TurboStreamResult TurboStreamResult(string action, string target, string viewName = null, object model = null) {
    ViewData.Model = model;

    return new TurboStreamResult(action, target)
    {
        ViewName = viewName,
        ViewData = ViewData,
        TempData = TempData
    };
}
```



