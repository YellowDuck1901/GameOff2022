using System.Collections;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Scriptable object which holds all the player's movement parameters. If you don't want to use it
    //just paste in all the parameters, though you will need to manuly change all references in this script
    public PlayerData Data;
    public PlayerSound _PlayerSound;
    public static bool _disableAllMovement, _disableJump, _disableLeft, _disableRight, _disableslide, _disableslideUp, _disableslideDown, _disableDash, _disableFaceLeft, _disableFaceRight;

    [SerializeField] private Animator _anim;

    #region COMPONENTS
    public Rigidbody2D RB { get; private set; }
    //Script to handle all player animations, all references can be safely removed if you're importing into your own project.
    //public PlayerAnimator AnimHandler { get; private set; }
    #endregion

    #region STATE PARAMETERS
    //Variables control the various actions the player can perform at any time.
    //These are fields which can are public allowing for other sctipts to read them
    //but can only be privately written to.
    public bool IsFacingRight { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsWallJumping { get; private set; }
    public bool IsDashing { get; private set; }
    public bool IsSliding { get; private set; }
    public bool IsRun { get; private set; }


    //Timers (also all fields, could be private and a method returning a bool could be used)
    public float LastOnGroundTime { get; private set; }
    public float LastOnWallTime { get; private set; }
    public float LastOnWallRightTime { get; private set; }
    public float LastOnWallLeftTime { get; private set; }

    //Jump
    private bool _isJumpCut;
    private bool _isJumpFalling;

    //Wall Jump
    private float _wallJumpStartTime;
    private int _lastWallJumpDir;

    //Dash
    [HideInInspector]
    public int _dashesLeft;
    private bool _dashRefilling;
    private Vector2 _lastDashDir;
    private bool _isDashAttacking;
    public bool _isPlayingDeadAnim;
    public GameObject outOfDash;
    public DashEffect dashEffect;

    // Input Slide: true when long-press key , false when un press
    private bool _isKeySlide;


    //Input Left: true when long-press, false when un press
    private bool _isKeyLeft;

    //Input Left: true when long-press, false when un press
    private bool _isKeyRight;


    [HideInInspector] public bool _isGrounded;
    [HideInInspector] public bool _isDashing;
    [HideInInspector] public bool _isRunning;
    [HideInInspector] public bool _isWallHang;
    [HideInInspector] public bool _isWallLeft;
    [HideInInspector] public bool _isWallRight;
    [HideInInspector] public bool _isDead;

    [HideInInspector] public bool _isAutoJumping;
    [HideInInspector] public bool _isAutoJumpLeft;
    [HideInInspector] public bool _isAutoJumpRight;


    public ParticleSystem dust;

    #endregion

    #region INPUT PARAMETERS
    private Vector2 _moveInput;

    public float LastPressedJumpTime { get; private set; }
    public float LastPressedDashTime { get; private set; }
    #endregion

    #region CHECK PARAMETERS
    //Set all of these up in the inspector
    [Header("Checks")]
    [SerializeField] private Transform _groundCheckPoint;
    //Size of groundCheck depends on the size of your character generally you want them slightly small than width (for ground) and height (for the wall check)
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
    [Space(5)]
    [SerializeField] private Transform _frontWallCheckPoint;
    [SerializeField] private Transform _backWallCheckPoint;

    [SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);

    [SerializeField] private Transform _AutoJumpWallCheckPoint;
    [SerializeField] private Vector2 _AutoJumpWallSize = new Vector2(0.5f, 1f);

    //[SerializeField] private Transform _AfterAutoJumpWallCheckPoint;
    //[SerializeField] private Vector2 _AfterAutoJumpWallSize = new Vector2(0.5f, 1f);

    #endregion

    #region LAYERS & TAGS
    [Header("Layers & Tags")]
    [SerializeField] private LayerMask _groundLayer;
    #endregion
    SpriteRenderer sr;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        //AnimHandler = GetComponent<PlayerAnimator>();
    }

    private void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        SetGravityScale(Data.gravityScale);
        IsFacingRight = true;
        outOfDash.SetActive(false);
    }

    private void Update()
    {

        #region TIMERS
        LastOnGroundTime -= Time.deltaTime;
        LastOnWallTime -= Time.deltaTime;
        LastOnWallRightTime -= Time.deltaTime;
        LastOnWallLeftTime -= Time.deltaTime;

        LastPressedJumpTime -= Time.deltaTime;
        LastPressedDashTime -= Time.deltaTime;
        #endregion

        #region INPUT HANDLER
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");

        if (_moveInput.x != 0)
            CheckDirectionToFace(_moveInput.x > 0);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.J))
        {
            OnJumpInput();
        }

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.J))
        {
            OnJumpUpInput();
        }

        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.K))
        {
            OnDashInput();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            _isKeySlide = true;
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            Mechanic.Countslide++;
            _isKeySlide = false;
        }

        if (!_isKeyLeft && Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            _isKeyLeft = true;
        }

        if (!_isKeyRight && Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            _isKeyRight = true;
        }

        if (_isKeyLeft && Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            _isKeyLeft = false;
            Mechanic.CountMoveLeft++;
        }

        if (_isKeyRight && Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            _isKeyRight = false;
            Mechanic.CountMoveRight++;
        }

        if (_isKeySlide && Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
        {
            Mechanic.CountslideUp++;
        }

        if (_isKeySlide && Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {
            Mechanic.CountslideDown++;
        }

        #endregion

        #region COLLISION CHECKS
        if (!IsDashing && !IsJumping)
        {
            //Ground Check
            if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer) && !IsJumping) //checks if set box overlaps with ground
            {
                if (LastOnGroundTime < -0.1f)
                {
                    //AnimHandler.justLanded = true;
                }

                LastOnGroundTime = Data.coyoteTime; //if so sets the lastGrounded to coyoteTime
                if (!_isGrounded)
                {
                    _isGrounded = true;
                    _anim.SetBool("Grounded", true);
                    _PlayerSound.playSoundLanding();
                }
           
            }
            else
            {
                if (_isGrounded)
                {
                    _anim.SetBool("Grounded", false);
                    _isGrounded = false;
                } 
                
            }

            if (_isGrounded)
            {
                _isWallLeft = false;
                _isWallRight = false;
                _isWallHang = false;

                _isAutoJumping = false;
                _isAutoJumpLeft = false;
                _isAutoJumpRight = false;

                outOfDash.SetActive(false);
            }


            //Right Wall Check
            if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)
                    || (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)) && !IsWallJumping)
            {
                LastOnWallRightTime = Data.coyoteTime;
                if (!_isGrounded && Input.GetKey(KeyCode.Z))
                {
                    _isWallHang = true;
                    _isWallRight = true;
                }
            }
            else if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)
                || (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)) && !IsWallJumping)
            {
                LastOnWallLeftTime = Data.coyoteTime;
                if (!_isGrounded && Input.GetKey(KeyCode.Z))
                {
                    _isWallHang = true;
                    _isWallLeft = true;

                }
            }
            else
            {
                _isWallLeft = false;
                _isWallRight = false;
                _isWallHang = false;
            }

            if (!(Physics2D.OverlapBox(_AutoJumpWallCheckPoint.position, _AutoJumpWallSize, 0, _groundLayer)) && _isWallHang)
            {
                Debug.Log("Un OverLapBox");
                _isAutoJumping = true;

                if ( _isWallLeft)
                {
                    _isAutoJumpLeft = true;
                }
                else if (_isWallRight)
                {
                    _isAutoJumpRight = true;
                }
            }else if(Physics2D.OverlapBox(_AutoJumpWallCheckPoint.position, _AutoJumpWallSize, 0, _groundLayer))
            {
                _isAutoJumping = false;
            }



                //if (!(Physics2D.OverlapBox(_AfterAutoJumpWallCheckPoint.position, _AfterAutoJumpWallSize, 0, _groundLayer)) )
                //{

                //}

                //Two checks needed for both left and rig   ht walls since whenever the play turns the wall checkPoints swap sides
                LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
        }
        #endregion

        #region JUMP CHECKS
        if (IsJumping && RB.velocity.y < 0)
        {
            IsJumping = false;

            if (!IsWallJumping)
                _isJumpFalling = true;
        }

        if (IsWallJumping && Time.time - _wallJumpStartTime > Data.wallJumpTime)
        {
            IsWallJumping = false;
        }

        if (LastOnGroundTime > 0 && !IsJumping && !IsWallJumping)
        {
            _isJumpCut = false;

            if (!IsJumping)
                _isJumpFalling = false;
        }

        if (!IsDashing)
        {
            //Jump
            if (CanJump() && LastPressedJumpTime > 0)
            {
                IsJumping = true;
                IsWallJumping = false;
                _isJumpCut = false;
                _isJumpFalling = false;
                Jump();

                //AnimHandler.startedJumping = true;
            }
            //WALL JUMP
            else if (CanWallJump() && LastPressedJumpTime > 0)
            {
                IsWallJumping = true;
                IsJumping = false;
                _isJumpCut = false;
                _isJumpFalling = false;

                _wallJumpStartTime = Time.time;
                _lastWallJumpDir = (LastOnWallRightTime > 0) ? -1 : 1;

                WallJump(_lastWallJumpDir);
            }
        }
        #endregion

        #region RUN CHECK
        if (_moveInput.x != 0 && LastOnGroundTime > 0)
        {
            IsRun = true;
        }else
        {
            IsRun = false;

        }
        #endregion

        #region DASH CHECKS
        if (CanDash() && LastPressedDashTime > 0 && !_isAutoJumping  && !_disableAllMovement && !_disableDash)
        {
            //Freeze game for split second. Adds juiciness and a bit of forgiveness over directional input
            Sleep(Data.dashSleepTime);

            //If not direction pressed, dash forward
            if (_moveInput != Vector2.zero)
                _lastDashDir = _moveInput;
            else
                _lastDashDir = IsFacingRight ? Vector2.right : Vector2.left;



            IsDashing = true;
            IsJumping = false;
            IsWallJumping = false;
            _isJumpCut = false;
            StartCoroutine(nameof(StartDash), _lastDashDir);
        }
        #endregion

        #region SLIDE CHECKS
        if (CanSlide() && ((LastOnWallLeftTime > 0 && _isKeySlide) || (LastOnWallRightTime > 0 && _isKeySlide)))
        {
            IsSliding = true;
        }
        else
            IsSliding = false;
        #endregion

        #region GRAVITY
        if (!_isDashAttacking)
        {
            //Higher gravity if we've released the jump input or are falling
            if (IsSliding)
            {
                SetGravityScale(0);
            }
            else if (RB.velocity.y < 0 && _moveInput.y < 0)
            {
                //Much higher gravity if holding down
                SetGravityScale(Data.gravityScale * Data.fastFallGravityMult);
                //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFastFallSpeed));
            }
            else if (_isJumpCut)
            {
                //Higher gravity if jump button released
                SetGravityScale(Data.gravityScale * Data.jumpCutGravityMult);
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
            }
            else if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
            {
                SetGravityScale(Data.gravityScale * Data.jumpHangGravityMult);
            }
            else if (RB.velocity.y < 0)
            {
                //Higher gravity if falling
                SetGravityScale(Data.gravityScale * Data.fallGravityMult);
                //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
            }
            else
            {
                //Default gravity if standing on a platform or moving upwards
                SetGravityScale(Data.gravityScale);
            }
        }
        else
        {
            //No gravity when dashing (returns to normal once initial dashAttack phase over)
            SetGravityScale(0);
        }

        AnimationController();


        Debug.Log("_isAutoJumpRight: " + _isAutoJumping);
        #endregion
    }

    private void FixedUpdate()
    {
        if (_disableAllMovement || _disableLeft && _moveInput.x <= 0)
        {
            _moveInput.x = 0;
            RB.velocity = new Vector2(0f, 0f);
        }

        else if (_disableAllMovement || _disableRight && _moveInput.x >= 0)
        {
            _moveInput.x = 0;
            RB.velocity = new Vector2(0f, 0f);
        }

        //Handle Run
        if (!IsDashing)
        {
            if (IsWallJumping)
                Run(Data.wallJumpRunLerp);
            else
                Run(1);
        }
        else if (_isDashAttacking)
        {
            Run(Data.dashEndRunLerp);
        }

        //Handle Slide
        if (IsSliding)
            Slide();

        if (!(IsDashing || IsJumping) && _isAutoJumping) //don't auto jump when dashing or jumping
        {
            if (_isAutoJumpLeft )
            {
                _moveInput.x = -1;
                Run(1);
            }
            else if (_isAutoJumpRight)
            {
                _moveInput.x = 1;
                Run(1);
            }
        }

        //if(!(IsDashing || IsJumping)) //don't auto jump when dashing or jumping
        //{
        //    //SlideOut();

        //    if (_isAutoJumpWallLeft)
        //    {
        //        _moveInput.x = -1;
        //        Run(1);
        //    }
        //    else if (_isAutoJumpWallRight)
        //    {
        //        _moveInput.x = 1;
        //        Run(1);
        //    }
        //}

    }


    #region INPUT CALLBACKS
    //Methods which whandle input detected in Update()
    public void OnJumpInput()
    {
        LastPressedJumpTime = Data.jumpInputBufferTime;
    }

    public void OnJumpUpInput()
    {
        if (CanJumpCut() || CanWallJumpCut())
            _isJumpCut = true;
    }

    public void OnDashInput()
    {
        LastPressedDashTime = Data.dashInputBufferTime;
    }

    #endregion

    #region GENERAL METHODS
    public void SetGravityScale(float scale)
    {
        RB.gravityScale = scale;
    }

    private void Sleep(float duration)
    {
        //Method used so we don't need to call StartCoroutine everywhere
        //nameof() notation means we don't need to input a string directly.
        //Removes chance of spelling mistakes and will improve error messages if any
        StartCoroutine(nameof(PerformSleep), duration);
    }

    private IEnumerator PerformSleep(float duration)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration); //Must be Realtime since timeScale with be 0 
        Time.timeScale = 1;
    }
    #endregion

    //MOVEMENT METHODS
    #region RUN METHODS
    private void Run(float lerpAmount)
    {
        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = _moveInput.x * Data.runMaxSpeed;
        //We can reduce are control using Lerp() this smooths changes to are direction and speed
        targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

        #region Calculate AccelRate
        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
        if (LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
        #endregion

        #region Add Bonus Jump Apex Acceleration
        //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
        {
            accelRate *= Data.jumpHangAccelerationMult;
            targetSpeed *= Data.jumpHangMaxSpeedMult;
        }
        #endregion

        #region Conserve Momentum
        //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        if (Data.doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
        {
            //Prevent any deceleration from happening, or in other words conserve are current momentum
            //You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
            accelRate = 0;
        }
        #endregion

        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - RB.velocity.x;
        //Calculate force along x-axis to apply to thr player

        float movement = speedDif * accelRate;

        //Convert this to a vector and apply to rigidbody
        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
        /*
		 * For those interested here is what AddForce() will do
		 * RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / RB.mass, RB.velocity.y);
		 * Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second
		*/
    }

    private void Turn()
    {
        //stores scale and flips the player along the x axis, 
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        IsFacingRight = !IsFacingRight;
    }
    #endregion

    #region JUMP METHODS
    private void Jump()
    {
        //Ensures we can't call Jump multiple times from one press
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;

        #region Perform Jump
        //We increase the force applied if we are falling
        //This means we'll always feel like we jump the same amount 
        //(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
        float force = Data.jumpForce;
        if (RB.velocity.y < 0)
            force -= RB.velocity.y;

        RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);

        //count number time of movement
        Mechanic.CountJump++;

        #endregion
    }

    private void WallJump(int dir)
    {
        //Ensures we can't call Wall Jump multiple times from one press
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;
        LastOnWallRightTime = 0;
        LastOnWallLeftTime = 0;

        #region Perform Wall Jump
        RB.velocity = new Vector2(0f, 0f);
        Vector2 force = new Vector2(Data.wallJumpForce.x, Data.wallJumpForce.y);
        force.x *= dir; //apply force in opposite direction of wall

        if (Mathf.Sign(RB.velocity.x) != Mathf.Sign(force.x))
            force.x -= RB.velocity.x;

        if (RB.velocity.y < 0) //checks whether player is falling, if so we subtract the velocity.y (counteracting force of gravity). This ensures the player always reaches our desired jump force or greater
            force.y -= RB.velocity.y;

        //Unlike in the run we want to use the Impulse mode.
        //The default mode will apply are force instantly ignoring masss
        RB.AddForce(force, ForceMode2D.Impulse);
        #endregion
        _PlayerSound.playSoundJump();
        Debug.Log("Wall jump");
    }
    #endregion

    #region DASH METHODS
    //Dash Coroutine
    private IEnumerator StartDash(Vector2 dir)
    {
        outOfDash.SetActive(true);
        _PlayerSound.playSoundDash();

        //Overall this method of dashing aims to mimic Celeste, if you're looking for
        // a more physics-based approach try a metho    d similar to that used in the jump

        LastOnGroundTime = 0;
        LastPressedDashTime = 0;

        float startTime = Time.time;

        _dashesLeft--;
        _isDashAttacking = true;

        SetGravityScale(0);

        //We keep the player's velocity at the dash speed during the "attack" phase (in celeste the first 0.15s)
        while (Time.time - startTime <= Data.dashAttackTime)
        {
            RB.velocity = dir.normalized * Data.dashSpeed;
            //Pauses the loop until the next frame, creating something of a Update loop. 
            //This is a cleaner implementation opposed to multiple timers and this coroutine approach is actually what is used in Celeste :D
            yield return null;
        }

        startTime = Time.time;

        _isDashAttacking = false;

        //Begins the "end" of our dash where we return some control to the player but still limit run acceleration (see Update() and Run())
        SetGravityScale(Data.gravityScale);
        RB.velocity = Data.dashEndSpeed * dir.normalized;

        while (Time.time - startTime <= Data.dashEndTime)
        {
            yield return null;
        }

        //Dash over
        IsDashing = false;

        //count number time of movement
        Mechanic.CountDash++;
    }

    //Short period before the player is able to dash again
    private IEnumerator RefillDash(int amount)
    {
        sr.color = Color.white;
        //SHoet cooldown, so we can't constantly dash along the ground, again this is the implementation in Celeste, feel free to change it up
        _dashRefilling = true;
        yield return new WaitForSeconds(Data.dashRefillTime);
        _dashRefilling = false;
        _dashesLeft = Mathf.Min(Data.dashAmount, _dashesLeft + 1);

    }
    #endregion

    #region Slide
    private void Slide()
    {
        //Works the same as the Run but only in the y-axis
        //THis seems to work fine, buit maybe you'll find a better way to implement a slide into this system
        //float speedDif = Data.slideSpeed - RB.velocity.y;

        //Me: Not apply velocity.y
        float speedDif = Data.slideSpeed;

        float movement = speedDif * Data.slideAccel;
        //So, we clamp the movement here to prevent any over corrections (these aren't noticeable in the Run)
        //The force applied can't be greater than the (negative) speedDifference * by how many times a second FixedUpdate() is called. For more info research how force are applied to rigidbodies.
        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

        //slide up
        if ((_moveInput.y > 0 && !_disableAllMovement && !_disableslideUp ) || _isAutoJumping )
        {
            RB.velocity = new Vector2(RB.velocity.x, 0f);
            RB.AddForce(movement * Vector2.up);
        }
        //slide down
        else if (_moveInput.y < 0 && !_disableAllMovement && !_disableslideDown)
        {
            RB.velocity = new Vector2(RB.velocity.x, 0f);
            RB.AddForce(movement * Vector2.down);
            
        }
        //hang
        else if (RB.velocity.y != 0 && !_disableAllMovement && !_disableslide) RB.velocity = new Vector2(RB.velocity.x, 0f);

        //autojump
        if (_isAutoJumping)
        {
            RB.velocity = new Vector2(RB.velocity.x, 0f);
            RB.AddForce(movement *(0.5f) * Vector2.up);
        }

        if (_isWallLeft)
        {
            CheckDirectionToFace(false);
        }
        else if (_isWallRight)
        {
            CheckDirectionToFace(true);

        }
    }

    //private void SlideOut()
    //{
    //    if (_isOutSlide && _isWallHang)
    //    {
    //        _isOutSlide = false;

    //        _isAutoJump = true;

    //        _isAutoJumping = true;
    //        RB.AddForce(new Vector2(0f, Data.SlideOutJumpForce.y), ForceMode2D.Impulse);
    //    }

    //    //Unlike in the run we want to use the Impulse mode.
    //    //The default mode will apply are force instantly ignoring masss
    //}

    #endregion


    #region CHECK METHODS
    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
        {
            if (IsFacingRight && !_disableAllMovement && !_disableFaceLeft) //turn left
            {
                Turn();

            }
            else if (!IsFacingRight && !_disableAllMovement && !_disableFaceRight)//turn right
            {
                Turn();
            }
        }
    }

    private bool CanJump()
    {
        return !_isAutoJumping && !_disableAllMovement && !_disableJump && LastOnGroundTime > 0 && !IsJumping;
    }

    private bool CanWallJump()
    {
        return !_isAutoJumping && !_disableAllMovement && !_disableJump && LastPressedJumpTime > 0 && LastOnWallTime > 0 && LastOnGroundTime <= 0 && (!IsWallJumping ||
             (LastOnWallRightTime > 0 && _lastWallJumpDir == 1) || (LastOnWallLeftTime > 0 && _lastWallJumpDir == -1));
    }

    private bool CanJumpCut()
    {
        return IsJumping && RB.velocity.y > 0;
    }

    private bool CanWallJumpCut()
    {
        return IsWallJumping && RB.velocity.y > 0;
    }

    private bool CanDash()
    {
        if ( !IsDashing && _dashesLeft < Data.dashAmount && LastOnGroundTime > 0 && !_dashRefilling)
        {
            StartCoroutine(nameof(RefillDash), 1);
        }

        return _dashesLeft > 0;
    }

    public bool CanSlide()
    {
        if ( !_disableAllMovement && !_disableslide  && LastOnWallTime > 0 && !IsJumping && !IsWallJumping && !IsDashing )
            return true;
        else
            return false;
    }
    #endregion


    #region EDITOR METHODS
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);  //draw cube describe for OverlapBox in region CollisionCheck
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_frontWallCheckPoint.position, _wallCheckSize); //draw cube describe for OverlapBox
        Gizmos.DrawWireCube(_backWallCheckPoint.position, _wallCheckSize);  //draw cube describe for OverlapBox
        Gizmos.DrawWireCube(_AutoJumpWallCheckPoint.position, _AutoJumpWallSize);  //draw cube describe for OverlapBox
        //Gizmos.DrawWireCube(_AfterAutoJumpWallCheckPoint.position, _AfterAutoJumpWallSize);  //draw cube describe for OverlapBox

    }
    #endregion

    void CreateDust()
    {
        dust.Play();
    }

    public void AnimationController()
    {
       
        if (_isDead)
        {
            if (!_isPlayingDeadAnim)
            {
                _isPlayingDeadAnim = true;
                _anim.SetTrigger("Dead");
                Manager_SFX.PlaySound_SFX(soundsGame.Dead,1f,1,128);
            }
            
        }

        if (!_disableAllMovement)
        {
            if (IsRun)
            {
                if(_moveInput.x > 0 && !_disableRight)
                {
                    _anim.SetBool("Moving", true);

                }
                else if(_moveInput.x < 0 && !_disableLeft)
                {
                    _anim.SetBool("Moving", true);

                }
                else
                {
                    _anim.SetBool("Moving", false);

                }
            }
            else
            {
                _anim.SetBool("Moving", false);
            }

            if (_isWallHang)
            {
                if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
                {
                    _anim.SetInteger("WallHang", 2);
                }
                else
                {
                    _anim.SetInteger("WallHang", 1);
                }
            }
            else
            {
                _anim.SetInteger("WallHang", 0);
            }

            if (!_disableJump)
            {
                if (_isGrounded && Input.GetKeyDown(KeyCode.C)
                    || _isGrounded && Input.GetKeyDown(KeyCode.Space))
                {
                    _anim.SetBool("Jumping", true);
                    CreateDust();

                }
                else
                {
                    _anim.SetBool("Jumping", false);
                }
            }
            else
            {
                _anim.SetBool("Jumping", false);
            }


            if (RB.velocity.y < 0 && !IsDashing && !_isWallHang)
            {
                _anim.SetBool("Falling", true);

            }
            if (_isGrounded || IsDashing || _isWallHang)
            {
                _anim.SetBool("Falling", false);
            }

            if (!_disableDash)
            {
                if (IsDashing)
                {
                    dashEffect.makeGhost = true;
                    
                    if ( (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow) )
                        || (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow)) )
                    {
                        _isDashing = true;
                        _anim.SetInteger("Dashing", 2);
                    }
                    else if (Input.GetKey(KeyCode.UpArrow) && _isGrounded)
                    {
                        _isDashing = true;
                        _anim.SetBool("Jumping", true);
                    }
                    else
                    {
                        _isDashing = true;
                        _anim.SetInteger("Dashing", 1);
                    }
                }
                else
                {
                    dashEffect.makeGhost = false;
                    _isDashing = false;
                    _anim.SetInteger("Dashing", 0);
                }
            }
            else
            {
                dashEffect.makeGhost = false;
                _isDashing = false;
                _anim.SetBool("Dashing", false);
            }

        }
        else
        {
            dashEffect.makeGhost = false;
            _isDashing = false;

            _anim.SetBool("Moving", false);
            _anim.SetBool("Dashing", false);
            _anim.SetBool("Jumping", false);
            _anim.SetBool("Falling", false);

            if(!_isDead)
            _anim.Play("Idle");
        }
    }

    
}

