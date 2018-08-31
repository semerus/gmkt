using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GiraffeStar
{
	public class AlphaGroup : MonoBehaviour {

		//public bool IsAbsolute;

		[SerializeField]
		float groupAlpha;
		public float GroupAlpha 
		{
			get { return groupAlpha; }
			set 
			{
				groupAlpha = value;
				ChangeAlphaInGroup ();
			}
		}

		void OnValidate()
		{
			groupAlpha = Mathf.Min (1f, Mathf.Max(0f, groupAlpha));
			ChangeAlphaInGroup ();
		}

		void ChangeAlphaInGroup()
		{
			var graphics = gameObject.GetComponentsInChildren<Graphic> ();
			foreach (var graphic in graphics)
			{
				var original = graphic.color;
				//var targetAlpha = IsAbsolute ? groupAlpha : graphic.color.a * groupAlpha;
				//relative 방식으로도 작동하면 좋겠지만 좋은 생각이 안나 패스
				var targetAlpha = groupAlpha;
				graphic.color = original.OverrideAlpha (targetAlpha);
			}
		}
	}
}

