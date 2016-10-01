using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class LoopScroll : MonoBehaviour
{
	[SerializeField] private RectTransform		m_Content;
	[SerializeField] private RectTransform[] 	m_PoolItems;
	[SerializeField] private bool				m_IsVertical;

	private Action<int, int> 					m_CallbackChanged;
	private Vector2 							m_ItemSize;

	private int									m_DataCount;
	private int									m_PoolItemCount;
	private int 								m_MoveIndex;

	public void Init( int dataCount, Action<int, int> callbackChanged )
	{
		m_CallbackChanged = callbackChanged;

		m_MoveIndex = 0;
		m_DataCount = dataCount;
		m_ItemSize = m_PoolItems[0].sizeDelta;
		m_PoolItemCount = m_PoolItems.Length;

		for( int i = 0; i < m_PoolItems.Length; i++ ) 
		{
			SetPositionPoolItem( i, i );
			if( m_CallbackChanged != null ) m_CallbackChanged( i, i );
		}
	}

	void Update()
	{
		if( m_PoolItemCount == 0 ) return;

		float itemSize 			= GetItemSize();
		float contentPosition 	= GetContentPosition();

		while( itemSize * ( m_MoveIndex + 2 ) < contentPosition )
		{
			int poolIndex 		= m_MoveIndex % m_PoolItemCount;
			int changedIndex 	= m_PoolItemCount + m_MoveIndex;
			int dataIndex 		= changedIndex % m_DataCount;

			if( m_MoveIndex < 0 ) 
			{
				poolIndex = GetReverseIndex( m_MoveIndex, m_PoolItemCount );
				dataIndex = GetReverseIndex( m_MoveIndex - ( m_DataCount - m_PoolItemCount ), m_DataCount );
			}

			SetPositionPoolItem( poolIndex, changedIndex );
			m_MoveIndex++;

			if( m_CallbackChanged != null )
			{
				m_CallbackChanged( poolIndex, dataIndex );
			}
		}

		while( itemSize * ( m_MoveIndex + 1 ) > contentPosition )
		{
			m_MoveIndex--;

			int poolIndex 		= m_MoveIndex % m_PoolItemCount;
			int changedIndex 	= m_MoveIndex;
			int dataIndex 		= changedIndex % m_DataCount;

			if( m_MoveIndex < 0 ) 
			{
				poolIndex = GetReverseIndex( m_MoveIndex, m_PoolItemCount );
				dataIndex = GetReverseIndex( m_MoveIndex, m_DataCount );
			}

			SetPositionPoolItem( poolIndex, changedIndex );

			if( m_CallbackChanged != null ) 
			{
				m_CallbackChanged( poolIndex, dataIndex );
			}
		}
	}

	private int GetReverseIndex( int cur, int max )
	{
		return ( max - 1 ) - ( Mathf.Abs( cur + 1 ) % max );
	}

	private float GetItemSize()
	{
		if( m_IsVertical )
		{
			return m_ItemSize.y;
		}

		return m_ItemSize.x;
	}

	private float GetContentPosition()
	{
		if( m_IsVertical )
		{
			return m_Content.anchoredPosition.y;
		}

		return -m_Content.anchoredPosition.x;
	}

	private void SetPositionPoolItem( int poolIndex, int changedIndex )
	{
		if( m_IsVertical ) 
		{
			m_PoolItems[ poolIndex ].anchoredPosition = new Vector2( 0.0f, -GetItemSize() * changedIndex );
		}
		else
		{
			m_PoolItems[ poolIndex ].anchoredPosition = new Vector2( GetItemSize() * changedIndex, 0.0f );
		}
	}

	public GameObject GetPoolItem( int index )
	{
		return m_PoolItems[ index ].gameObject;
	}
}
