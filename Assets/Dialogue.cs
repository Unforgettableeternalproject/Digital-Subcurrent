using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Digital_Subcurrent 
{
    [System.Serializable]
    public class Dialogue
    {
        public string sequenceName;

        [TextArea(3, 10)]
        public string[] sentences;

        // «Øºc¦¡
        public Dialogue(string name, string[] sentences)
        {
            this.sequenceName = name;
            this.sentences = sentences;
        }
    }
}