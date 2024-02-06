using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ioni;
using UnityEngine;

namespace FGJ24.Stats
{
    [Serializable]
    public class VariableList : List<Variable>
    {
        
    }
    
    public class VariableHolder : MonoBehaviour
    {
        [SerializeField] private List<Var> variableConfigurations;

        [SerializeField] private List<Variable> runtimeVariables;

        [SerializeField] private List<IntVariable> runtimeInts;

        private void Start()
        {
            var intVars = variableConfigurations.VarsOfType<IntVar>();
            intVars.ForEach(var =>
            {
                var intVariable = (IntVariable) var.Variable;
                intVariable.SetDefaultValue(var);
                runtimeInts.Add(intVariable);
            });
            
            var newList = new List<Variable>();
            variableConfigurations.ForEach(conf =>
            {
                if (conf.GetType == VariableType.Int)
                {
                    var intVariable = new IntVariable(((IntVar)conf).DefaultValue);
                    newList.Add(intVariable);
                }
                if (conf.GetType == VariableType.String)
                {
                    var stringVariable = new StringVariable(((StringVar)conf).DefaultValue);
                    newList.Add(stringVariable);
                }
            });
            runtimeVariables = newList;
            
            D.Info("RuntimeVar Count", runtimeVariables);
        }
    }
}
