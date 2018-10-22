# MoviesMobileApp

Application is developed by using Xamarin.Forms framework. Android platform is currently supported only. Minimum sdk version - 18. Application is designed for smartphones primarily. Portrait and landscape orientations are supported.

There are no special build instructions. You should use Release mode to create signed APK file.

Third-party libraries used:
- Akavache (to cache data, simple and effective key-value store)
- Automapper (to map one object to another)
- Fody and PropertyChanged.Fody (to add property changed notification to all classes that implement INotifyPropertyChanged)
- modernhttpclient (to use platform-specific networking libraries with HttpClient for better performance)
- NUnit (to write and run unit tests)
- Polly (to use network operations policies sush as Retry and Fault-handling)
- Prism.DryIoc.Forms (to integrate Prism and DryIoc easily)
- Prism.Forms (to build loosely coupled, maintainable MVVM application, including such possibilities as Commands, DI, Navigation and others)
- DryIoc (to add DI for project, small and fast Ioc container)
- Refit (to integrate REST api easily)
- System.Reactive.Linq (to express complex event processing queries over observable sequences)
- Xam.Plugin.Connectivity (to get network connectivity information)
- FFImageLoading (to image loading, caching and provide better performance during images usage)