# GDI+를 이용해 보자
이번 장에서는 Windows Forms에서 GDI+를 자유롭게 이용할 수 있도록 

## 테스트 환경
- Visual Studio 2019
- .NET 5, C# 9
- Windows Forms

## GDI+ 많이는 들어봤는데...
Windows 초창기 부터 그래픽 장치 인터페이스(GDI) API를 제공 했었는데요, Windows XP 운영체제부터 Windows GDI+를 사용할 수 있게 되었습니다. GDI+는 GDI에 비해 그라데이션 브러쉬 및 알파 블렌딩과 같은 새로운 기능이 추가되었고, 친숙한 개체지향 스타일로 각종 기능을 사용할 수 있게 되었습니다.

## Windows Forms에서 GDI+를 사용하려면?
두가지 방식이 있습니다. Windows Forms의 화면에 보여지는 모든 컨트롤들은 `OnPaintBackground()`및 `OnPaint()` 메소드를 재정의 할 수 있습니다. 이때, `e` 인자의 Graphics 개체를 통해 GDI+ 기능을 이용할 수 있습니다.
두번째 방법은 `CreateGraphics()` 메소드를 통해 `Graphics` 개체를 생성한 후 사용하는 방법입니다.
아, 한가지 방법이 더 있군요. `Bitmap` 개체를 `Graphics.FromImage()` 메소드를 통해 `Graphics`를 획득해서 사용할 수 있습니다.

## `Graphics`는 화면 출력 용으로만 사용하나요?
아닙니다. GDI+는 장치 컨텍스트에 독립적입니다. 그렇기 때문에 프린트 출력을 할때에도 `Graphics`를 사용하게 됩니다.

## 한번 해봅시다
먼저 `.NET 5`로 `Windows Forms` 응용 어플리케이션 템플릿으로 프로젝트를 생성한 후, 아무 컨트롤이나 폼에 배치해봅니다.

![기본 폼에 컨트롤 배치](images/1.png)

예시로 `CheckBox`를 생성해봤는데요, Windows Forms에서는 화면에 보이는 모든 컨트롤은 `Control`에서 상속받도록 되어 있습니다. 한번 추적해보죠.

```csharp
public class CheckBox : ButtonBase {}
public abstract class ButtonBase : Control {}
```

`OnPaintBackground()`및 `OnPaint()` 메소드는 `Control`의 메소드 이므로, Windows Forms의 보여지는 모든 컨트롤은 그리기 기능이 존재하는 샘입니다.

그렇다면 `Control`에서 상속받아 구현을 해볼까요? 이름을 `CanvasControl`이라고 하겠습니다.

```csharp
    public class CanvasControl : Control
    {
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;

            g.Clear(Color.Black);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
    }
```

폼 디자이너에서 컨트롤이 구분되도록 배경색을 검정색으로 칠하는 코드만 넣었습니다. 그런 다음 폼 디자이너에서 `CanvasControl`을 추가하면 다음처럼 보이게 됩니다.

![폼에 CanvasControl1](images/2.png)

실행화면1

![CanvasControl 실행화면](images/3.png)

컨트롤이 화면에 보여져야 할 때 먼저 `OnPaintBackground()`가 호출되어 배경을 그릴 수 있도록 하고, 그다음 `OnPaint()`가 호출되어 내용을 그릴 수 있게 되어 있습니다. 이런식으로 Windows Forms 컨트롤이 화면에 보여지게 되는거죠.

그런데 검정 배경화면만 있으니 `BackColor`에 검정 지정한 것 마냥 차이가 없습니다. 검정색 배경에 도형을 그려보도록 합시다.

```csharp
    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        var rect = ClientRectangle;
        var clipRect = e.ClipRectangle;

        // 선
        g.DrawLine(Pens.Red, 50, 50, 100, 100);

        // 면
        g.DrawRectangle(Pens.Green, 10, 10, 30, 30);

        // 원
        g.DrawEllipse(Pens.Blue, 100, 100, 50, 50);
    }
```

실행화면2

![CanvasControl 실행화면](images/4.png)

원하는대로 잘 그려지는군요. 그런데 한가지 궁금한 점이 생깁니다. 

## `OnPaint()`는 정확히 언제 호출될까요?
Windows Forms는 나름 효과적으로 컨트롤을 그리도록 최적화 되어 있는데요, 화면에 새로 보여질 때만 호출된다는 점입니다. 다음의 코드로 이를 확인해봅시다.

```csharp
    private bool firstDraw = true;
    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        var rect = ClientRectangle;

        // 선
        g.DrawLine(Pens.Red, 50, 50, 100, 100);

        // 면
        g.DrawRectangle(Pens.Green, 10, 10, 30, 30);

        // 원
        g.DrawEllipse(Pens.Blue, 100, 100, 50, 50);

        if (firstDraw == false)
            g.FillRectangle(Brushes.LightGray, rect);

        firstDraw = false;
    }
```

`firstDraw`에 의해 두번째 그리기부터는 전체 영역을 회색으로 그리도록 했는데요,

최초화면

![최초화면](images/5.png)

폼의 사이즈를 작게 조절해서 다음과 같이 줄인 후,

![폼사이즈줄임](images/6.png)

![폼사이즈늘림](images/7.png)

신기합니다. 분명히 컨트롤 전체 영역에 갈색으로 칠해져야 할 텐데요, 다시 그려져야 할 영역만 칠하고 있습니다. `OnPaint()` 메소드가 호출될 때의 `Graphics`는 갱신되어야 할 영역만 그려지도록 클리핑(Clipping) 됨을 알 수 있습니다.

```csharp
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var rect = ClientRectangle;

            g.FillEllipse(Brushes.Green, rect);
        }
```

위와 같이 수정한 후 `CanvasControl`을 폼에 `Dock.Fill` 해 봅시다.

실행화면

![녹색원](images/8.png)

`ClientRectangle`은 컨트롤의 그릴 수 있는 영역입니다. 예상했던 대로 녹색으로 가득 찬 원이 그려졌는데요, 사이즈를 한번 늘려봅시다.

![이상한 녹색원](images/9.png)

원이 크기에 맞게 늘어나거나 줄어드는 예상과 달리 Windows Forms의 효율적 그리기로 인해서 이상하게 보입니다.

컨트롤의 사이즈가 변경되었을 때 전체를 다시 그려야 할 경우,

```csharp
        public CanvasControl()
        {
            ResizeRedraw = true;
        }
```
이렇게 `ResizedRedraw` 속성에 `true`를 적용하면,

![괜찮은 녹색원](images/10.png)

컨트롤의 사이즈가 변경됐을 때 전체 영역을 다시 그리게 됩니다.

## GDI+ 개체에 대해 알아봅시다
- `Pen` : 선의 형태를 결정합니다. 색, 너비 그리고 모양을 표현할 수 있습니다.
- `Brush` : 면의 형태를 결정합니다. SolidBrush, HatchBrush, TextureBrush, LinearGradientBrush, PathGradientBrush 등 다양한 `Brush` 형태를 이용할 수 있습니다.
- 



## 샘플

## 참고
- [Windows Forms의 그래픽 및 그리기](https://docs.microsoft.com/ko-kr/dotnet/desktop/winforms/advanced/graphics-and-drawing-in-windows-forms?view=netframeworkdesktop-4.8)

## 문서 기여자
- 작성한 사람: 디모이(dimohy)