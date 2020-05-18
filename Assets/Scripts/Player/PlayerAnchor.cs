using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnchor : MonoBehaviour
{
    public static System.Action<int> OnCoinsUpdated; //вызывается когда начисляются очки
    public static System.Action OnGameOver; //вызывается когда игра оканчивается

    [SerializeField] private float MaxSideDisplacement = 3f; //максимальное смещение на которое капля может 
    [SerializeField] private float MergerBorder = 0.55f; //!!!также устанавливается в аниматоре

    [SerializeField] private float ForwardSpeed, SideSpeed;

    [SerializeField] private GameObject LeftDrop, RightDrop; //правая и левая каплю
    private SpriteRenderer leftDropRenderer, rightDropRenderer;

    private Animator animator;

    private enum MovementState
    {
        MoveForward,
        MoveCurve
    }
    private MovementState movementState = MovementState.MoveForward;

    private float sideOffset;

    private int coinNumber;

    //параметры для движения по кривой
    private Vector2 pipeStartPoint, pipeControlPoint, pipeEndPoint;
    private Quaternion pipeStartRotation, pipeEndRotation;
    private float time;

    //привести в порядок выше описанное /|\

    private void Awake()
    {
        animator = GetComponent<Animator>();

        //кэширование данных о каплях
        leftDropRenderer = LeftDrop.GetComponent<SpriteRenderer>();
        rightDropRenderer = RightDrop.GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        //движение игрока
        if (movementState == MovementState.MoveForward)
        {
            //движение вперёд
            transform.position += transform.up * ForwardSpeed * Time.fixedDeltaTime;
        }
        else if (movementState == MovementState.MoveCurve)
        {
            float t = (Mathf.PI / 6) * ForwardSpeed * (Time.time - time); //по формуле должно быть Mathf.PI / 4 но игрок движется в 1.5 раза быстрее

            Vector2 playerPosition = GetBezQuadPoint(pipeStartPoint, pipeControlPoint, pipeEndPoint, t);
            Quaternion playerRotation = Quaternion.Lerp(pipeStartRotation, pipeEndRotation, t);

            //движение по кривой безье и поворот
            transform.position = playerPosition;
            transform.rotation = playerRotation;

            //при полном повороте игрока на 90 градусов он продолжает движение прямо
            if (transform.rotation.eulerAngles.z % 90.0f == 0.0f) //Math.Round((Mathf.Abs(transform.rotation.eulerAngles.z) % 90.0f), 3) == 0.0f
            {
                StopMoveCurve();
            }
        }
    }

    public void Die()
    {
        //принудительная остановка движения
        //---

        OnGameOver.Invoke();
    }

    public void AddCoin()
    {
        coinNumber++;

        OnCoinsUpdated.Invoke(coinNumber);
    }

    public void SideMovement(float normalizedValue)
    {
        //получение нормализованного смещения в сторону
        sideOffset += normalizedValue * SideSpeed * Time.timeScale; //Time.timeScale не позваляет двигаться во время паузы
        sideOffset = Mathf.Clamp(sideOffset, 0f, MaxSideDisplacement);

        //Использования смещения
        //transform.position = transform.right * sideOffset;
        LeftDrop.transform.localPosition = new Vector2(-1 * sideOffset, 0);
        RightDrop.transform.localPosition = new Vector2(1 * sideOffset, 0);

        //анимация будет проходить в зависимости от нормального значения свайпа
        animator.SetFloat("Offset", sideOffset);

        //значение в if говорит о том насколько должны разъехаться капли что бы они стали 2-умя объетами
        if (sideOffset >= MergerBorder)
        {
            GetComponent<SpriteRenderer>().enabled = false;

            rightDropRenderer.enabled = true;
            leftDropRenderer.enabled = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = true;

            rightDropRenderer.enabled = false;
            leftDropRenderer.enabled = false;
        }
    }

    public void AbortSideMovement(float normalizedValue)
    {
        if (sideOffset < MergerBorder)
        {
            //получение нормализованного смещения в сторону
            sideOffset -= normalizedValue * SideSpeed * Time.timeScale;
            sideOffset = Mathf.Clamp(sideOffset, 0f, MaxSideDisplacement);

            //Использования смещения
            //transform.position = transform.right * sideOffset;
            LeftDrop.transform.localPosition = new Vector2(-1 * sideOffset, 0);
            RightDrop.transform.localPosition = new Vector2(1 * sideOffset, 0);

            //анимация будет проходить в зависимости от нормального значения свайпа
            animator.SetFloat("Offset", sideOffset);
        }
    }

    public void StartMoveCurve(Transform startPoint, Transform сontrolPoint, Transform endPoint)
    {
        movementState = MovementState.MoveCurve;

        InitBezPoints(startPoint, сontrolPoint, endPoint);
    }

    private void StopMoveCurve()
    {
        movementState = MovementState.MoveForward;

        //Можно было бы вызывать остановку движения по кривой при OnTriggerExit у поворота, но коллайдер поворота и игрока может быть разным
    }

    private void InitBezPoints(Transform startPoint, Transform сontrolPoint, Transform endPoint)
    {
        time = Time.time;

        //точки для расчёта движения по кривой Безье
        pipeStartPoint = startPoint.position;
        pipeControlPoint = сontrolPoint.position;
        pipeEndPoint = endPoint.position;

        //кваторнионы для поворота игрока
        pipeStartRotation = startPoint.rotation;
        pipeEndRotation = endPoint.rotation;
    }

    //можно вынести в другой скрипт
    private Vector2 GetBezQuadPoint(Vector2 p0, Vector2 p1, Vector2 p2, float t) //нахождение кривой безье
    {
        Vector2 q0 = Vector2.Lerp(p0, p1, t);
        Vector2 q1 = Vector2.Lerp(p1, p2, t);

        Vector2 b = Vector2.Lerp(q0, q1, t);

        return b; //позиция игрока на кривой
    }
}
