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
        public StructureItem next, previous;
        public StructureItem(StructureItem previous)
        {
            if (previous != null)
            {
                this.previous = previous;
                previous.next = this;
            }
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
            float? minimum = null;
            foreach (Node node in nodes)
            {
                float value = 0.0f;
                if (node.fromWhereCame != null)
                {

                    float prevMinimum = 0;
                    if (node.fromWhereCame.owner.previous != null)
                        prevMinimum = node.fromWhereCame.owner.previous.GetMinimum();
                    
                    value = (prevMinimum + node.weightToCome);
                    //if (node.owner.minimumLetter != null)
                    //    value /= Alphabet.GetProbability(node.currentLetter, node.owner.minimumLetter.currentLetter);

                    if (minimum == null)
                    {
                        minimum = value;
                        minimumLetter = node;
                    }
                    else
                    {
                        if (minimum > value)
                        //if (((minimum > value) && (value != 0)) || (minimum == 0 && minimumLetter == null))
                        {
                            minimum = value;
                            minimumLetter = node;
                        }
                    }
                    
                }
            }
            if (minimum == null)
            {
                return 0;
            }
            else
            {
                return minimum.Value;
            }
        }

        public string GetMinimumWord()
        {
            string recognizedText = "";
            if (minimumLetter != null && minimumLetter.fromWhereCame != null && minimumLetter.fromWhereCame.owner.previous != null)
            {
                recognizedText = recognizedText + minimumLetter.fromWhereCame.owner.previous.GetMinimumWord();
                recognizedText = recognizedText + Alphabet.GetLetter(minimumLetter.currentLetter);
            }
            else
            {
                if (minimumLetter != null && minimumLetter.fromWhereCame != null )
                {
                    recognizedText = recognizedText + minimumLetter.fromWhereCame.owner.GetMinimumWord();
                    recognizedText = recognizedText + Alphabet.GetLetter(minimumLetter.currentLetter);
                }
            }
            return recognizedText;

        }
    }
}
