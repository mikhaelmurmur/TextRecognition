using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRecognition
{
    class StructureItem
    {
        public List<Node> nodes = new List<Node>();
        public StructureItem()
        {
            for (int letterIndex = 0; letterIndex < Alphabet.GetAlphabetLength(); letterIndex++)
            {
                Node letter = new Node((Letters)letterIndex, Alphabet.GetLetterPath((Letters)letterIndex), this);
                nodes.Add(letter);
            }
        }
        float minimum;
        bool isMinimumEvaluated = false;
        Node minimumLetter;

        public float GetMinimum()
        {
            if (isMinimumEvaluated)
            {
                return minimum;
            }
            else
            {
                minimum = EvaluateMinimum();
                isMinimumEvaluated = true;
                return minimum;
            }
        }

        private float EvaluateMinimum()
        {
            float minimum = 0;
            foreach (Node node in nodes)
            {
                float value = 0.0f;
                if (node.fromWhereCame != null)
                {
                    float prevMinimum = node.fromWhereCame.owner.GetMinimum();
                    value = (prevMinimum + node.weightToCome);
                    if (node.owner.minimumLetter != null)
                        value /= Alphabet.GetProbability(node.currentLetter, node.owner.minimumLetter.currentLetter);
                }
                if (((minimum > value) && (value != 0)) || (minimum == 0))
                {
                    minimum = value;
                    minimumLetter = node;
                }
            }
            return minimum;
        }

        public string GetMinimumWord()
        {
            string recognizedText = "";
            if (minimumLetter != null && minimumLetter.fromWhereCame != null)
            {
                recognizedText = recognizedText + minimumLetter.fromWhereCame.owner.GetMinimumWord();
                recognizedText = recognizedText + Alphabet.GetLetter(minimumLetter.currentLetter);
            }
            return recognizedText;

        }
    }
}
