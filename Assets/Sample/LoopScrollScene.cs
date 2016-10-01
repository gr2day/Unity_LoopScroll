using UnityEngine;
using System.Collections;

public class LoopScrollScene : MonoBehaviour
{
	private readonly string[] DATA_LIST = new string[]
	{
		"Item01", "Item02", "Item03", "Item04", "Item05",
		"Item06", "Item07", "Item08", "Item09", "Item10",
	};

	[SerializeField] private LoopScroll m_VerticalLoopScroll;
	[SerializeField] private LoopScroll m_HorizontalInfiniteScroll;

	// Use this for initialization
	void Start () 
	{
		m_VerticalLoopScroll.Init( 
			DATA_LIST.Length, 
			delegate( int poolIndex, int dataIndex )
			{
				Item item = m_VerticalLoopScroll.GetPoolItem( poolIndex ).GetComponent<Item>();
				item.Init( DATA_LIST[dataIndex] );
			}
		);

		m_HorizontalInfiniteScroll.Init( 
			DATA_LIST.Length, 
			delegate( int poolIndex, int dataIndex )
			{
				Item item = m_HorizontalInfiniteScroll.GetPoolItem( poolIndex ).GetComponent<Item>();
				item.Init( DATA_LIST[dataIndex] );
			}
		);
	}
}
