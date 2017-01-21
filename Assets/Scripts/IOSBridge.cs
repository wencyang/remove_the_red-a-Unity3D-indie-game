using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class IOSBridge
{
	[DllImport ("__Internal")]
	private static extern void _AddNotification(
		string title,
		string body,
		string cancelLabel,
		string firstLabel,
		string secondLabel);

	public static void AddNotification(
		string title,
		string body,
		string cancelLabel,
		string firstLabel,
		string secondLabel)
	{
		_AddNotification (
			 title,
			 body,
			 cancelLabel,
			 firstLabel,
			secondLabel);
	}

}
