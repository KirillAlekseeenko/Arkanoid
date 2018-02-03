using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Records : IEnumerable
{
	public int MaxSessionCount { get{ return 10; } }

	[SerializeField] private List<Session> sessionList;

	public Records()
	{
		sessionList = new List<Session>();
	}
	public void AddSession(Session session)
	{
		int index = 0;
		while (index < sessionList.Count && sessionList[index].Score > session.Score) {
			index++;
		}
		if (index < MaxSessionCount) {
			sessionList.Insert (index, session);
			for (int i = MaxSessionCount; i < sessionList.Count; i++) {
				sessionList.RemoveAt (i);
			}
		}
	}

	public Session this[int index] { get { return sessionList [index]; } }
	public int Count { get{ return sessionList.Count; } }

	#region IEnumerable implementation

	public IEnumerator GetEnumerator ()
	{
		return sessionList.GetEnumerator ();
	}

	#endregion
}
