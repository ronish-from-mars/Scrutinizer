namespace FlatFiles.Scrutinizer.Validators
{
    using System.Collections.Generic;

    public class Pipeline<T> where T : IResult
    {
        private readonly List<IFilter<T>> _filters = new List<IFilter<T>>();

        public Pipeline<T> Register(IFilter<T> filter)
        {
            _filters.Add(filter);
            return this;
        }

        public List<T> Execute(T input)
        {
            var results = new List<T>();
            foreach (var item in _filters)
            {
                var result = item.Execute(input);
                if (result.StopExecution)
                {
                    results.Add(result);
                    return results;
                }
                else
                {
                    continue;
                }
            }

            return results;
        }
    }
}
