using UnityEngine;

// От B0N3head 
// Всё ваше, используйте этот скрипт как хотите, не стесняйтесь указывать авторство, если хотите
[AddComponentMenu("Движение игрока и управление камерой")]
public class PlayerMovement : MonoBehaviour
{
    [Header("Настройки камеры")] [Tooltip("Закрепить курсор на экране игры во время игры")] [SerializeField]
    private bool lockCursor = true;

    [Tooltip("Ограничить угол камеры (остановить камеру от \"скручивания шеи\")")] [SerializeField]
    private Vector2 clampInDegrees = new Vector2(360f, 180f);

    [Tooltip("Чувствительность мыши, по оси x и y")] [SerializeField]
    private Vector2 sensitivity = new Vector2(2f, 2f);

    [Tooltip("Сглаживание движения мыши (попробуйте с ним и без него)")] [SerializeField]
    private Vector2 smoothing = new Vector2(1.5f, 1.5f);

    [Tooltip("Должно иметь то же имя, что и ваша основная камера")] [SerializeField]
    private string cameraName = "Камера";

    //----------------------------------------------------
    [Space] [Header("Настройки движения")] [Tooltip("Максимальная скорость ходьбы")] [SerializeField]
    private float walkMoveSpeed = 7.5f;

    [Tooltip("Максимальная скорость спринта")] [SerializeField]
    private float sprintMoveSpeed = 11f;

    [Tooltip("Максимальная скорость прыжка")] [SerializeField]
    private float jumpMoveSpeed = 6f;

    [Tooltip("Максимальная скорость при приседании")] [SerializeField]
    private float crouchMoveSpeed = 4f;

    //----------------------------------------------------
    [Header("Настройки приседания")] [Tooltip("Время, необходимое для приседания")] [SerializeField]
    private float crouchDownSpeed = 0.2f;

    [Tooltip("Какой высоты персонаж, когда он приседает")] [SerializeField]
    private float crouchHeight = 0.68f; // измените для желаемого размера при приседании

    [Tooltip("Какой высоты персонаж, когда он стоит")] [SerializeField]
    private float standingHeight = 1f;

    [Tooltip("Плавный переход между приседанием и стоянием")] [SerializeField]
    private bool smoothCrouch = true;

    [Tooltip("Можно ли приседать в воздухе")] [SerializeField]
    private bool jumpCrouching = true;

    //----------------------------------------------------
    [Header("Настройки прыжка")] [Tooltip("Начальная сила прыжка")] [SerializeField]
    private float jumpForce = 110f;

    [Tooltip("Непрерывная сила прыжка")] [SerializeField]
    private float jumpAccel = 10f;

    [Tooltip("Максимальное время прыжка вверх")] [SerializeField]
    private float jumpTime = 0.4f;

    [Tooltip("Как долго можно прыгать после выхода с края (в секундах)")] [SerializeField]
    private float coyoteTime = 0.2f;

    [Tooltip("Как долго я должен буферизовать ваш ввод прыжка (в секундах)")] [SerializeField]
    private float jumpBuffer = 0.1f;

    [Tooltip("Как долго я должен ждать перед следующим прыжком")] [SerializeField]
    private float jumpCooldown = 0.6f;

    [Tooltip("Ускоренное падение")] [SerializeField]
    private float extraGravity = 0.1f;

    [Tooltip("Тег, который будет считаться землей")] [SerializeField]
    private string groundTag = "Земля";

    //----------------------------------------------------
    [Space] [Header("Настройки клавиатуры")] [Tooltip("Клавиша для прыжка")] [SerializeField]
    private KeyCode jump = KeyCode.Space;

    [Tooltip("Клавиша для спринта")] [SerializeField]
    private KeyCode sprint = KeyCode.LeftShift;

    [Tooltip("Клавиша для приседания")] [SerializeField]
    private KeyCode crouch = KeyCode.Z;

    [Tooltip("Клавиша для переключения курсора")] [SerializeField]
    private KeyCode lockToggle = KeyCode.Q;

    //----------------------------------------------------
    [Space] [Header("Отладочная информация")] [Tooltip("Мы на земле?")] [SerializeField]
    private bool areWeGrounded = true;

    [Tooltip("Мы приседаем?")] [SerializeField]
    private bool areWeCrouching = false;

    [Tooltip("Текущая скорость, с которой я должен двигаться")] [SerializeField]
    private float currentSpeed;

    //----------------------------------------------------
    // Ссылочные переменные (эти переменные используются в расчетах, их не нужно задавать пользователю)
    private Rigidbody rb;
    private GameObject cam;
    Vector3 input = new Vector3();
    Vector2 _mouseAbsolute, _smoothMouse, targetDirection, targetCharacterDirection;
    private float coyoteTimeCounter, jumpBufferCounter, startJumpTime, endJumpTime;
    private bool wantingToJump = false, wantingToCrouch = false, wantingToSprint = false, jumpCooldownOver = true;

    public bool IsStop { get; set; } = false;

    private void Awake()
    {
        // Просто установите rb на Rigidbody объекта, содержащего этот скрипт
        rb = gameObject.GetComponent<Rigidbody>();
        // Попробуйте найти нашу камеру среди дочерних объектов
        cam = gameObject.transform.Find(cameraName).gameObject;
        // Установите currentSpeed на скорость ходьбы, так как еще не нажаты клавиши
        currentSpeed = walkMoveSpeed;

        // Установите целевое направление на начальную ориентацию камеры.
        targetDirection = transform.localRotation.eulerAngles;
        // Установите целевое направление для тела персонажа в его начальное состояние.
        targetCharacterDirection = transform.localRotation.eulerAngles;
    }

    private void Update()
    {
        if (IsStop) return;

        // Обновите позицию камеры
        cameraUpdate();

        // Переместите все вводимые данные в Update(), затем используйте введенные данные в FixedUpdate()

        // Движение WSAD
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        // Клавиша прыжка
        wantingToJump = Input.GetKey(jump);
        // Клавиша приседания
        wantingToCrouch = Input.GetKey(crouch);
        // Клавиша спринта
        wantingToSprint = Input.GetKey(sprint);

        // Переключение блокировки мыши (KeyDown срабатывает только один раз)
        if (Input.GetKeyDown(lockToggle))
            lockCursor = !lockCursor;
    }

    public void cameraUpdate()
    {
        // Позволяет скрипту ограничивать значение на основе желаемого целевого значения.
        var targetOrientation = Quaternion.Euler(targetDirection);
        var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

        // Получить необработанный ввод мыши для более чистого считывания на более чувствительных мышах.
        var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        // Масштабируйте ввод в соответствии с настройкой чувствительности и умножьте это на значение сглаживания.
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

        // Интерполируйте движение мыши с течением времени, чтобы применить сглаживание.
        _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
        _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

        // Найдите абсолютное значение движения мыши от нулевой точки.
        _mouseAbsolute += _smoothMouse;

        // Ограничьте и примените локальное значение x сначала, чтобы не быть затронутым мировыми преобразованиями.
        if (clampInDegrees.x < 360)
            _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

        // Затем ограничьте и примените глобальное значение y.
        if (clampInDegrees.y < 360)
            _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

        cam.transform.localRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right) *
                                      targetOrientation;

        var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, Vector3.up);
        transform.localRotation = yRotation * targetCharacterOrientation;
    }

    private void FixedUpdate()
    {
        if (IsStop) return;

        // Обработка блокировки курсора
        if (lockCursor)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;

        // Двойная проверка, находимся ли мы на земле или нет (изменяет текущую скорость, если верно)
        // --- КРАТКОЕ ОБЪЯСНЕНИЕ --- 
        // transform.position.y - transform.localScale.y + 0.1f
        // Это ставит начало луча на 0.1f выше нижней части игрока
        // Затем мы стреляем лучом вниз на 0.15f, это позволяет игроку с 0.5f попадать в объекты
        // Удаление этого +- 0.1f и стрельба прямо под игроком может пропустить землю, так как иногда нижняя часть капсулы проходит сквозь землю
        if (Physics.Raycast(
                new Vector3(transform.position.x, transform.position.y - transform.localScale.y + 0.1f,
                    transform.position.z), Vector3.down, 0.15f))
            handleHitGround();

        // Спринт
        if (wantingToSprint && areWeGrounded && !areWeCrouching)
            currentSpeed = sprintMoveSpeed;
        else if (!areWeCrouching && areWeGrounded)
            currentSpeed = walkMoveSpeed;

        // Приседание 
        // Можно упростить до Crouch((wantingToCrouch && jumpCrouching)); хотя ниже более читаемо
        if (wantingToCrouch && jumpCrouching)
            Crouch(true);
        else
            Crouch(false);

        // Таймер Койота (Когда игрок покидает землю, начинаем отсчет от установленного значения coyoteTime)
        // Это позволяет игрокам прыгать поздно. После того как они покинули 
        if (areWeGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        // Таймер буфера прыжка (Когда игрок покидает землю, начинаем отсчет от установленного значения jumpBuffer)
        // Это "буферизует" ввод и позволяет ранним нажатиям пробела быть действительными и больше не игнорироваться
        if (wantingToJump)
            jumpBufferCounter = jumpBuffer;
        else
            jumpBufferCounter -= Time.deltaTime;

        // Если таймер Койота не истек и наш буфер прыжка не истек, и наш таймер ожидания (canJump) теперь закончился
        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && jumpCooldownOver)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            jumpCooldownOver = false;
            areWeGrounded = false;
            jumpBufferCounter = 0f;
            currentSpeed = jumpMoveSpeed;
            endJumpTime = Time.time + jumpTime;

            // Ждем jumpCooldown (1f = 1 секунда), затем запускаем void jumpCoolDownCountdown()
            Invoke(nameof(jumpCoolDownCountdown), jumpCooldown);
        }
        else if (wantingToJump && !areWeGrounded && endJumpTime > Time.time)
        {
            // Удерживайте пробел для дальнейшего прыжка (до истечения таймера)
            rb.AddForce(Vector3.up * jumpAccel, ForceMode.Acceleration);
        }

        // Движение WSAD
        input = input.normalized;
        Vector3 forwardVel = transform.forward * currentSpeed * input.z;
        Vector3 horizontalVel = transform.right * currentSpeed * input.x;
        rb.velocity = horizontalVel + forwardVel + new Vector3(0, rb.velocity.y, 0);

        // Дополнительная гравитация для более плавного прыжка
        rb.AddForce(new Vector3(0, -extraGravity, 0), ForceMode.Impulse);
    }

    private void jumpCoolDownCountdown()
    {
        jumpCooldownOver = true;
    }

// Обработка приседания
    private void Crouch(bool crouch)
    {
        areWeCrouching = crouch;

        if (crouch)
        {
            // Если игрок приседает
            currentSpeed = crouchMoveSpeed;

            if (smoothCrouch)
            {
                transform.localScale = new Vector3(transform.localScale.x,
                    Mathf.Lerp(transform.localScale.y, crouchHeight, crouchDownSpeed), transform.localScale.z);
                transform.position = Vector3.Lerp(transform.position,
                    new Vector3(transform.position.x, transform.position.y - crouchHeight, transform.position.z),
                    crouchDownSpeed);
            }
            else if (transform.localScale != new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z))
            {
                transform.localScale = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);
                transform.position = new Vector3(transform.position.x, transform.position.y - crouchHeight / 2,
                    transform.position.z);
            }
        }
        else
        {
            // Если игрок стоит
            if (smoothCrouch)
            {
                transform.localScale = new Vector3(transform.localScale.x,
                    Mathf.Lerp(transform.localScale.y, standingHeight, crouchDownSpeed), transform.localScale.z);
                transform.position = Vector3.Lerp(transform.position,
                    new Vector3(transform.position.x, transform.position.y - standingHeight / 2, transform.position.z),
                    crouchDownSpeed);
            }
            else if (transform.localScale !=
                     new Vector3(transform.localScale.x, standingHeight, transform.localScale.z))
            {
                transform.localScale = new Vector3(transform.localScale.x, standingHeight, transform.localScale.z);
                transform.position = new Vector3(transform.position.x, transform.position.y + standingHeight / 2,
                    transform.position.z);
            }
        }
    }

// Проверка на землю
//****** убедитесь, что то, что вы хотите считать землей в вашей игре, соответствует тегу, установленному в скрипте
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == groundTag)
            handleHitGround();
    }

// Этот код отделен в отдельный метод, так как его нужно выполнять в двух разных случаях; это экономит время на копирование и вставку кода
// Просто двойная проверка, если мы приседаем и установка скорости соответственно 
    public void handleHitGround()
    {
        if (areWeCrouching)
            currentSpeed = crouchMoveSpeed;
        else
            currentSpeed = walkMoveSpeed;

        areWeGrounded = true;
    }

// Не беспокойтесь о понимании этого; это просто код для настройки персонажа игрока
    public void setupCharacter()
    {
        gameObject.tag = "Player";
        if (!gameObject.GetComponent<Rigidbody>())
        {
            Rigidbody rb = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
            rb.mass = 10;
        }
        else Debug.Log("Rigidbody already exists");

        if (!gameObject.transform.Find("Camera"))
        {
            Vector3 old = transform.position;
            gameObject.transform.position = new Vector3(0, -0.8f, 0);
            GameObject go = new GameObject("Camera");
            go.AddComponent<Camera>();
            go.AddComponent<AudioListener>();
            go.transform.rotation = new Quaternion(0, 0, 0, 0);
            go.transform.localScale = new Vector3(1, 1, 1);
            go.transform.parent = transform;
            gameObject.transform.position = old;
            Debug.Log("Camera created");
        }
        else Debug.Log("Camera already exists");
    }
}