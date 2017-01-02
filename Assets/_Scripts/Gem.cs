using UnityEngine;
using UnityEngine.UI;
using System;


namespace game 
{

    public class Gem : MonoBehaviour
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
        
        private Vector2 directionChosen;


        public void Init(int c_, int r_)
        {
            c = c_;
            r = r_;

            this.transform.localPosition = new Vector2(c * (Gem.SIZE + Gem.GAP) + GameControl.GRID_X,
                                                     (GameControl.ROWS - r) * (Gem.SIZE + Gem.GAP) + GameControl.GRID_Y);

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
            return new Vector2(c * (Gem.SIZE + Gem.GAP) + GameControl.GRID_X, r * (Gem.SIZE + Gem.GAP) + GameControl.GRID_Y);
        }


        // Drop newly created gem 
        public void DoDrop(float delay_)
        {
            //TODO: optimisation note; Only trigger OnGemLanded when last gem has landed 

            // Move Example
            LeanTween.moveLocal(this.gameObject, GetVisualPosition(c, 0 - r), GameControl.SECS_PER_ROW * r)
                                .setDelay(delay_)
                                .setEase(LeanTweenType.easeInSine)
                                .setOnComplete( () => {  OnGemLanded(new EventArgs()); }
                               );
            
        }

        // Drop gem already on grid 
        public void DoDropDown(float delay_, int lR)
        {
            LeanTween.moveLocal(this.gameObject, GetVisualPosition(c, 0 - lR), (lR - r) * GameControl.SECS_PER_ROW)
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
                        //startPos = touch.position;
                        // directionChosen = false;
                        print("TOUCH BEGIN");
                        break;

                    // Determine direction by comparing the current touch position with the initial one.
                    case TouchPhase.Moved:
                        //direction = touch.position - startPos;
                        // Ensure movement is sufficent
                        /*
                        if (Math.abs(m.x) + Math.abs(m.y) < 2)
                            return;

                        // Detect direction
                        if (Math.abs(m.x) > Math.abs(m.y))
                            dX = (m.x > 0 ? 1 : -1);
                        else
                            dY = (m.y > 0 ? 1 : -1);

                        _game.dispatchEventWith(MOVED, false, { c: dX, r: dY } );
                        */
                        break;

                    // Report that a direction has been chosen when the finger is lifted.
                    case TouchPhase.Ended:
                        // directionChosen = true;
                        break;
                }
            }

            //if(Input.GetMouseButtonDown(0))
            //if (directionChosen)
            //{
            // Something that uses the chosen direction...
            // }
        }

        void OnMouseDown()
        {
            OnGemSelected(new GemSelectedEventArgs(this));
        }

        protected virtual void OnGemLanded(EventArgs e)
        {
            GemLandedHandler handler = GemLanded;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public delegate void GemLandedHandler(object sender, EventArgs e);
        public event GemLandedHandler GemLanded;


        protected virtual void OnGemSelected(GemSelectedEventArgs e)
		{
            GemSelectedHandler handler = GemSelected;
            if (handler != null)
            {
                handler(this, e);
            }
		}
        public delegate void GemSelectedHandler(object sender, GemSelectedEventArgs e);
        public event GemSelectedHandler GemSelected;
	}

    public class GemSelectedEventArgs : EventArgs
    {
        public Gem gem { get; private set; }

        public GemSelectedEventArgs(Gem gem_)
        {
            gem = gem_;
        }
    }
}
 