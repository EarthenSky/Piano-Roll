Imports System.IO

'1 is start of note, 2 is body, 3 is end, 0 is no note.  All else is just a temp colour.
Enum BlockValue
    Empty = 0
    Start = 1
    Body = 2
    Close = 3  'End
    StartColour = 4
    BodyColour = 5
    CloseColour = 6
End Enum

'Holds note values in hz
Enum Note
    B7 = 3951
    B6 = 1976
    Ash6 = 1865
    A6 = 1760
    B5 = 988
    Gsh5 = 831
    G5 = 784
    Fsh5 = 740
    F5 = 698
    E5 = 659
    Dsh5 = 622
    D5 = 587
    Csh5 = 554
    C5 = 523
    B4 = 494
    Ash4 = 466
    A4 = 440
    B3 = 247
    B2 = 123
End Enum

Public Class Form1
    'Piano Roll 
    'Gabe Stang
    '
    'Play a piano on the piano roll

    Const shtBlockSizeX As Short = 8
    Const shtBlockSizeY As Short = 8

    Private blnIsStartMoving As Boolean = False
    Private blnIsCloseMoving As Boolean = False
    Private blnIsBodyMoving As Boolean = False

    Private pntLastMousePos As Point = New Point(0, 0)

    Private currentFileDirectory As String = Directory.GetCurrentDirectory.Remove(Directory.GetCurrentDirectory.IndexOf("\bin\Debug"), 10) + "\"

    Private imgBackgroundTiles As Image = Image.FromFile(currentFileDirectory & "Grid of tiles.png")
    Private imgRedTile As Image = Image.FromFile(currentFileDirectory & "RedTile.png")
    Private imgBrownTile As Image = Image.FromFile(currentFileDirectory & "BrownTile.png")
    Private imgTealTile As Image = Image.FromFile(currentFileDirectory & "TealTile.png")

    Private arrayNoteValue(511, 59) As BlockValue 'Holds values of notes

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub PaintNotes(ByVal o As Object, ByVal e As PaintEventArgs) Handles pbxGrid.Paint
        e.Graphics.DrawImage(Image.FromFile(currentFileDirectory & "Grid of tiles.png"), Point.Empty)

        'Problem? VVV  Fixed... i think.
        For xPos As Integer = 0 To arrayNoteValue.GetLength(0) - 1 'goes through each value and sets the colour
            For yPos As Integer = 0 To arrayNoteValue.GetLength(1) - 1
                If arrayNoteValue(xPos, yPos) = BlockValue.Empty Then
                    Continue For 'TOME: Arbitrary but nice to see for parsing through code.
                ElseIf arrayNoteValue(xPos, yPos) = BlockValue.Start OrElse arrayNoteValue(xPos, yPos) = BlockValue.StartColour Then
                    e.Graphics.DrawImage(imgRedTile, New Point(xPos * shtBlockSizeX, yPos * shtBlockSizeY))
                ElseIf arrayNoteValue(xPos, yPos) = BlockValue.Body OrElse arrayNoteValue(xPos, yPos) = BlockValue.BodyColour Then
                    e.Graphics.DrawImage(imgBrownTile, New Point(xPos * shtBlockSizeX, yPos * shtBlockSizeY))
                ElseIf arrayNoteValue(xPos, yPos) = BlockValue.Close OrElse arrayNoteValue(xPos, yPos) = BlockValue.CloseColour Then
                    e.Graphics.DrawImage(imgTealTile, New Point(xPos * shtBlockSizeX, yPos * shtBlockSizeY))
                End If
            Next yPos
        Next xPos
    End Sub

    Private Sub MouseUpClick(ByVal sender As System.Object, ByVal e As MouseEventArgs) Handles pbxGrid.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Dim xIndex = (e.X + 0) \ shtBlockSizeX
            Dim yIndex = (e.Y + 0) \ shtBlockSizeY

            If blnIsStartMoving = True Then 'TODO: what do I do in this case?
                arrayNoteValue(pntLastMousePos.X, pntLastMousePos.Y) = BlockValue.Start
                blnIsStartMoving = False

            End If

        End If
    End Sub

    Private Sub MouseDownClick(ByVal sender As System.Object, ByVal e As MouseEventArgs) Handles pbxGrid.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Dim xIndex = (e.X + 0) \ shtBlockSizeX
            Dim yIndex = (e.Y + 0) \ shtBlockSizeY
            Me.Text = "X: " & xIndex & " Y: " & yIndex

            'tests
            Select Case (yIndex)
                Case 0
                    PlaySound(Note.Csh5, 50)
                Case 1
                    PlaySound(Note.C5, 50)
                Case 2
                    PlaySound(Note.B4, 50)
                Case 3
                    PlaySound(Note.Ash4, 50)
                Case 4
                    PlaySound(Note.A4, 50)
                Case 5
                Case 6
                Case 7
                Case 8
                Case 9
                Case 10
                Case 11
                Case 12
            End Select

            If arrayNoteValue(xIndex, yIndex) = BlockValue.Empty Then
                tbxDebugLog.Text += " 0"
                'places a note at the specific place clicked
                FindAndDeleteNote(xIndex + 1, yIndex)
                arrayNoteValue(xIndex + 1, yIndex) = BlockValue.Close
                arrayNoteValue(xIndex, yIndex) = BlockValue.Start

            ElseIf arrayNoteValue(xIndex, yIndex) = BlockValue.Start Then
                tbxDebugLog.Text += " 1"
                'changes the colour of the start block
                arrayNoteValue(xIndex, yIndex) = BlockValue.BodyColour  'So you know you clicked it.  TODO: add new tinted colours.
                pntLastMousePos = New Point(xIndex, yIndex)
                blnIsStartMoving = True

            End If
        End If
        Refresh()
    End Sub

    ''' <summary>
    ''' Looks for a note in the containing space and deletes all reference of it.
    ''' </summary>
    Private Sub FindAndDeleteNote(ByVal xIndex As Short, ByVal yIndex As Short)
        Dim shtCurrentPlace As Short = 0
        If arrayNoteValue(xIndex, yIndex) <> 0 Then
            'check down the row.  If it is a part of a note, delete it.  Stop when it hits the end of the note.

            If arrayNoteValue(xIndex, yIndex) = 1 Then
                While arrayNoteValue(xIndex + shtCurrentPlace, yIndex) <> 3
                    arrayNoteValue(xIndex + shtCurrentPlace, yIndex) = 0
                    shtCurrentPlace += 1

                End While
                arrayNoteValue(xIndex + shtCurrentPlace, yIndex) = 0

            ElseIf arrayNoteValue(xIndex, yIndex) = 2 Then
                'check up the row.  If it is a part of a note, delete it.  
                'When it hits the start of the note go back to the middle and start down the note until it hits the end of the note. 

                'deletes the 3
                arrayNoteValue(xIndex + shtCurrentPlace, yIndex) = 0
                shtCurrentPlace -= 1

                While arrayNoteValue(xIndex + shtCurrentPlace, yIndex) <> 0
                    If arrayNoteValue(xIndex + shtCurrentPlace, yIndex) = 2 Then
                        arrayNoteValue(xIndex + shtCurrentPlace, yIndex) = 0
                        shtCurrentPlace -= 1

                    ElseIf arrayNoteValue(xIndex + shtCurrentPlace, yIndex) = 1 Then
                        arrayNoteValue(xIndex + shtCurrentPlace, yIndex) = 0
                        shtCurrentPlace = 0
                        Exit While

                    End If
                End While

                While arrayNoteValue(xIndex + shtCurrentPlace, yIndex) <> 0
                    If arrayNoteValue(xIndex + shtCurrentPlace, yIndex) = 2 Then
                        arrayNoteValue(xIndex + shtCurrentPlace, yIndex) = 0
                        shtCurrentPlace += 1

                    ElseIf arrayNoteValue(xIndex + shtCurrentPlace, yIndex) = 3 Then
                        arrayNoteValue(xIndex + shtCurrentPlace, yIndex) = 0
                        Exit While

                    End If
                End While

            ElseIf arrayNoteValue(xIndex, yIndex) = 3 Then
                'Check up the row.  If it is a part of a note, delete it.  When it hits the start of the note stop. 

                'deletes the 3
                arrayNoteValue(xIndex + shtCurrentPlace, yIndex) = 0
                shtCurrentPlace -= 1

                While arrayNoteValue(xIndex + shtCurrentPlace, yIndex) <> 0
                    If arrayNoteValue(xIndex + shtCurrentPlace, yIndex) = 2 Then
                        arrayNoteValue(xIndex + shtCurrentPlace, yIndex) = 0
                        shtCurrentPlace -= 1

                    ElseIf arrayNoteValue(xIndex + shtCurrentPlace, yIndex) = 1 Then
                        arrayNoteValue(xIndex + shtCurrentPlace, yIndex) = 0
                        shtCurrentPlace = 0
                        Exit While

                    End If
                End While
            End If
        End If
    End Sub

    ''' <summary>
    ''' Plays a sound based on the note and the length
    ''' </summary>
    ''' <param name="length">Input a number like 7/32</param>
    Private Sub PlaySound(ByVal note As Note, ByVal length As Short)
        Console.Beep(note, length)
    End Sub

End Class

'//           ▄███▄  /
'//           ██▄██▄ -- <Quack!>
'//       ▄█▄▄▄███▄  \
'//       ██████████
'//       ▀███████▀
'//   ________________________
'//  |CODE DUCK STRIKES AGAIN!|
'//  |________________________|
