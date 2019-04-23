using System.Collections.Generic;

namespace Chess.Common.System
{
    public class RuleSet 
    {
        public delegate bool PerformsMoveIf(Board board, Move move);
        public string RuleSetName { get; }
        private readonly Dictionary<string, PerformsMoveIf> _rules = new Dictionary<string, PerformsMoveIf>();

        public RuleSet(string ruleSetName)
        {
            RuleSetName = ruleSetName;
        }

        public RuleSet Add(string ruleName, PerformsMoveIf rule)
        {
            _rules.Add(ruleName, rule);
            return this;
        }

        public IEnumerable<RuleResult> Check(Board board, Move move)
        {
            var list = new List<RuleResult>();
            foreach (var performsMoveIf in _rules)
            {
                var passed = performsMoveIf.Value(board, move);

                // TODO: Handle this better, do I even want to log here as we pass the results back anyway?
//                Logger.Log($"{RuleSetName}.{performsMoveIf.Key} = {passed}");

                list.Add(passed 
                    ? RuleResult.Pass 
                    : RuleResult.Fail(performsMoveIf.Key)
                );
            }

            return list;
        }

        public class RuleResult
        {
            public static RuleResult Pass => new RuleResult();
            public static RuleResult Fail(string ruleId) => new RuleResult(false, ruleId);

            public string RuleId { get; }
            public bool Passed { get; }

            private RuleResult(bool passed, string ruleId)
            {
                Passed = passed;
                RuleId = ruleId;
            }

            private RuleResult()
            {
                Passed = true;
            }

            public override string ToString() 
                => $"{(Passed ? "passed" : "failed")} - {RuleId}";
        }
    }
}