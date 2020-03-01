using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// 事件触发监听
///     功能：实现对于任何对象的监听处理。不支持拖动事件监听。
/// </summary>
public class EventTriggerListener :MonoBehaviour,IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
        public delegate void VoidDelegate(object args=null);
        public VoidDelegate onClick;
        public VoidDelegate onDown;
        public VoidDelegate onEnter;
        public VoidDelegate onExit;
        public VoidDelegate onUp;
        public VoidDelegate onSelect;
        public VoidDelegate onUpdateSelect;

    private object parameter;
    //public delegate void ObjectClick(object args);
    //public ObjectClick onObjectClick;
    //public static EventTriggerListener GetClick(GameObject go,object obj)
    //{
    //    EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
    //    if (listener == null)
    //    {
    //        listener = go.AddComponent<EventTriggerListener>();
    //        listener.par = obj;
    //    }
    //    return listener;
    //}

        /// <summary>
        /// 得到“监听器”组件
        /// </summary>
        /// <param name="go">监听的游戏对象</param>
        /// <returns>
        /// 监听器
        /// </returns>
        public static EventTriggerListener Get(GameObject go,object obj=null)
        {
            EventTriggerListener lister = go.GetComponent<EventTriggerListener>();
            if (lister==null)
            {
                lister = go.AddComponent<EventTriggerListener>();
                lister.parameter = obj;
            }
            return lister;
        }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null)
        {
            onClick(parameter);
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (onDown != null)
            {
                onDown(parameter);
            }
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (onEnter != null)
            {
                onEnter(parameter);
            }
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (onExit != null)
            {
                onExit(parameter);
            }
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (onUp != null)
            {
                onUp(parameter);
            }
        }
    
        public virtual void OnSelect(BaseEventData eventBaseData)
        {
            if (onSelect != null)
            {
                onSelect(parameter);
            }
        }

        public virtual void OnUpdateSelected(BaseEventData eventBaseData)
        {
            if (onUpdateSelect != null)
            {
                onUpdateSelect(parameter);
            }
        }
	
    }//Class_end

