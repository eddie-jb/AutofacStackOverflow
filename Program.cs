namespace AutofacStackOverflow {
  using Autofac;

  interface IAmATest<T> { }

  class TestStackOverflow
    : IAmATest<int>,
      IAmATest<string>,
      IAmATest<double> { }

  class TestStackOverflowDecorator<T> : IAmATest<T> {
    public TestStackOverflowDecorator(IAmATest<T> test) { }
  }

  class Program {
    static void Main(string[] args) {
      var builder = new ContainerBuilder();

      builder.RegisterAssemblyTypes(typeof(Program).Assembly).AsClosedTypesOf(typeof(IAmATest<>));

      builder.RegisterGenericDecorator(
        typeof(TestStackOverflowDecorator<>),
        typeof(IAmATest<>),
        ctx => !ctx.ImplementationType.IsGenericType ||
               ctx.ImplementationType.GetGenericTypeDefinition() != typeof(TestStackOverflowDecorator<>));

      var container = builder.Build();
      var test = container.Resolve<IAmATest<int>>();
    }
  }
}