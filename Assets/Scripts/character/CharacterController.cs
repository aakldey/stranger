using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour
{
    //переменная для установки макс. скорости персонажа
    public float maxSpeed = 10f;

    public float jumpForce = 600.0f;
    //переменная для определения направления персонажа вправо/влево
    public bool isFacingRight = true;
	public bool isFacingUp = true;
    //ссылка на компонент анимаций
    private Animator anim;
    //находится ли персонаж на земле или в прыжке?
    private bool isGrounded = false;
    //ссылка на компонент Transform объекта
    //для определения соприкосновения с землей
    public Transform groundCheck;
    //радиус определения соприкосновения с землей
    private float groundRadius = 0.2f;
    //ссылка на слой, представляющий землю
    public LayerMask whatIsGround;
    
    public bool enableControl = false;

	public bool ladder = false;

    PlayerState playerState;

	public float gravityScale = 1.0f;

	public GameObject lever = null;

    /// <summary>
    /// Начальная инициализация
    /// </summary>
    private void Start()
    {
        playerState = GetComponent<PlayerState>();
        //whatIsGround = LayerMask.NameToLayer(playerState.CurrentState.ToString());
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Выполняем действия в методе FixedUpdate, т. к. в компоненте Animator персонажа
    /// выставлено значение Animate Physics = true и анимация синхронизируется с расчетами физики
    /// </summary>
    private void FixedUpdate()
    {
        if (enableControl)
        {
            //определяем, на земле ли персонаж
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
            //устанавливаем соответствующую переменную в аниматоре
            //anim.SetBool("Ground", isGrounded);
            //устанавливаем в аниматоре значение скорости взлета/падения
            //anim.SetFloat("vSpeed", rigidbody2D.velocity.y);
            //если персонаж в прыжке - выход из метода, чтобы не выполнялись действия, связанные с бегом
            //if (!isGrounded)
            //    return;
            //используем Input.GetAxis для оси Х. метод возвращает значение оси в пределах от -1 до 1.
            //при стандартных настройках проекта 
            //-1 возвращается при нажатии на клавиатуре стрелки влево (или клавиши А),
            //1 возвращается при нажатии на клавиатуре стрелки вправо (или клавиши D)
            float move = Input.GetAxis("Horizontal");
			float l = Input.GetAxis("Vertical");

            //в компоненте анимаций изменяем значение параметра Speed на значение оси Х.
            //приэтом нам нужен модуль значения
            //anim.SetFloat("Speed", Mathf.Abs(move));

			if (ladder)
			{
				rigidbody2D.gravityScale = 0;
				rigidbody2D.velocity = new Vector2(0, 0);
			}
			else
			{
				rigidbody2D.gravityScale = gravityScale;
				
			}
			if(rigidbody2D.velocity.y > 0.01)
				isFacingUp = true; 
			else
				isFacingUp = false;

            //обращаемся к компоненту персонажа RigidBody2D. задаем ему скорость по оси Х, 
            //равную значению оси Х умноженное на значение макс. скорости
            rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);

			if (ladder)
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, maxSpeed*l);

            //если нажали клавишу для перемещения вправо, а персонаж направлен влево
            if (move > 0 && !isFacingRight)
                //отражаем персонажа вправо
                Flip();
            //обратная ситуация. отражаем персонажа влево
            else if (move < 0 && isFacingRight)
                Flip();

		
        }
    }

    private void Update()
    {
        if (enableControl)
        {
			
			if (Input.GetButtonDown("Use"))
			{
				if (lever != null) {
					if(!lever.GetComponent<Lever>().busy) {
						lever.GetComponent<Lever>().Switch();
					}
				}
			}

            //если персонаж на земле и нажат пробел...
            if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                //устанавливаем в аниматоре переменную в false
                //anim.SetBool("Ground", false);
                //прикладываем силу вверх, чтобы персонаж подпрыгнул
                rigidbody2D.AddForce(new Vector2(0, jumpForce));
            }
        }
    }

    /// <summary>
    /// Метод для смены направления движения персонажа и его зеркального отражения
    /// </summary>
    private void Flip()
    {
        //меняем направление движения персонажа
        isFacingRight = !isFacingRight;
        //получаем размеры персонажа
        Vector3 theScale = transform.localScale;
        //зеркально отражаем персонажа по оси Х
        theScale.x *= -1;
        //задаем новый размер персонажа, равный старому, но зеркально отраженный
        transform.localScale = theScale;
    }
}