using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;


namespace game 
{

    public class Gem : MonoBehaviour, IDragHandler
    {
        public Sprite[] gemImages;

        private int _type;
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }

        public int c, r;    // Column, Row
        private Image _image;

        private const int SIZE = 35;
        private const int GAP = 6;

        public const int TYPES = 5;
        public const float SWAP_ANI_TIME = 0.3f;
        
        private Vector2 directionChosen, startPos;


        public void Init(int c_, int r_)
        {
            c = c_;
            r = r_;

            this.transform.localPosition = new Vector2(c * (Gem.SIZE + Gem.GAP) + Grid.WIDTH,
                                                     (Grid.ROWS - r) * (Gem.SIZE + Gem.GAP) + Grid.HEIGHT);

            _image = this.GetComponent<Image>();
        }

        public int ColourType
        {
            get
            {
                return(type);
            }
            set
            {
                type = value;
                // Set image to match gem type
                _image.sprite = gemImages[type - 1];
                this.transform.localScale = Vector3.one;
                _image.SetNativeSize();
            }
            
        }

        // Return position on game area
        public static Vector2 GetVisualPosition(int c, int r)
        {
            return new Vector2(c * (Gem.SIZE + Gem.GAP) + Grid.WIDTH, r * (Gem.SIZE + Gem.GAP) + Grid.HEIGHT);
        }


        // Drop newly created gem 
        public void DoDrop(float delay_)
        {
            //TODO: optimisation note; Only trigger OnGemLanded when last gem has landed 

            // Move Example
            LeanTween.moveLocal(this.gameObject, GetVisualPosition(c, 0 - r), Grid.SECS_PER_ROW * r)
                                .setDelay(delay_)
                                .setEase(LeanTweenType.easeInSine)
                                .setOnComplete( () => {  OnGemLanded(new EventArgs()); }
                               );
            
        }

        // Drop gem already on grid 
        public void DoDropDown(float delay_, int lR)
        {
            LeanTween.moveLocal(this.gameObject, GetVisualPosition(c, 0 - lR), (lR - r) * Grid.SECS_PER_ROW)
                                .setDelay(delay_)
                                .setEase(LeanTweenType.easeInSine);
        }

        void Update()
        {
            // Track a single touch as a direction control.
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                // Handle finger movements based on touch phase.
                switch (touch.phase)
                {
                    // Record initial touch position.
                    case TouchPhase.Began:
                        startPos = touch.position;
                        directionChosen = Vector2.zero;
                        print("TOUCH BEGIN");
                        break;

                    // Determine direction by comparing the current touch position with the initial one.
                    case TouchPhase.Moved:
                        // Ensure movement is sufficent
                        directionChosen = startPos - touch.position;
                        if( directionChosen.magnitude < 2)
                            return;

                        //TODO: Detect direction
                        /*
                        if (Mathf.abs(m.x) > Mathf.abs(m.y))
                            dX = (m.x > 0 ? 1 : -1);
                        else
                            dY = (m.y > 0 ? 1 : -1);

                        _game.dispatchEventWith(MOVED, false, { c: dX, r: dY } );
                        */
                        break;
                        
                }
            }
            
        }

        void OnMouseDown()
        {
            OnGemSelected(new GemSelectedEventArgs(this));
        }

        public void OnDrag(PointerEventData data)
        {
            //OnGemSelected(new GemSelectedEventArgs(null));
            if (data.dragging)
            {
                if (Mathf.Abs(data.delta.x) > Mathf.Abs(data.delta.y))
                    directionChosen = (data.delta.x > 0 ? Vector2.right : Vector2.left);
                else
                    directionChosen = (data.delta.x > 0 ? Vector2.up : Vector2.down);

                OnGemDragged(new GemDraggedEventArgs(this, directionChosen));
            }
        }

        public event GemLandedHandler GemLanded;
        protected virtual void OnGemLanded(EventArgs e)
        {
            GemLandedHandler handler = GemLanded;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public delegate void GemLandedHandler(object sender, EventArgs e);
        


        
        protected virtual void OnGemSelected(GemSelectedEventArgs e)
		{
            GemSelectedHandler handler = GemSelected;
            if (handler != null)
            {
                handler(this, e);
            }
		}
        public event GemSelectedHandler GemSelected;
        public delegate void GemSelectedHandler(object sender, GemSelectedEventArgs e);
        

        public event GemDraggedHandler GemDragged;
        protected virtual void OnGemDragged(GemDraggedEventArgs e)
        {
            GemDraggedHandler handler = GemDragged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public delegate void GemDraggedHandler(object sender, GemDraggedEventArgs e);
        
    }

    public class GemSelectedEventArgs : EventArgs
    {
        public Gem gem { get; private set; }

        public GemSelectedEventArgs(Gem gem_)
        {
            gem = gem_;
        }
    }

    public class GemDraggedEventArgs : EventArgs
    {
        public Gem gem { get; private set; }
        public Vector2 dragDirection { get; private set; }

        public GemDraggedEventArgs(Gem gem_, Vector2 dragDirection_)
        {
            gem = gem_;
            dragDirection = dragDirection_;
        }
    }
}
 