using Godot;

public partial class Player : CharacterBody3D
{
    [Export] public float Speed = 6f;
    [Export] public float Gravity = 20f;
    [Export] public float MouseSensitivity = 0.003f;

    private Camera3D _camera;

    public override void _Ready()
    {
        GD.Print("Player ready");
        _camera = GetNode<Camera3D>("Camera3D");
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion motion)
        {
            RotateY(-motion.Relative.X * MouseSensitivity);
            float pitch = _camera.Rotation.X - motion.Relative.Y * MouseSensitivity;
            _camera.Rotation = new Vector3(Mathf.Clamp(pitch, -1.5f, 1.5f), 0f, 0f);
        }

        if (@event.IsActionPressed("ui_cancel"))
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;

        if (!IsOnFloor())
        {
            velocity.Y -= Gravity * (float)delta;
        }

        Vector2 input = Input.GetVector("left", "right", "forward", "back");
        Vector3 direction = (Transform.Basis * new Vector3(input.X, 0f, input.Y)).Normalized();

        velocity.X = direction.X * Speed;
        velocity.Z = direction.Z * Speed;

        Velocity = velocity;
        MoveAndSlide();
    }
}