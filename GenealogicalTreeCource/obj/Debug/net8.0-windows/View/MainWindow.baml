   M S B A M L   `   `   `  ����Q  MGenealogicalTreeCource, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:GenealogicalTreeCource.ClassGenealogicalTreeCource.Class  5   	   '    !GenealogicalTreeCource.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5   	   6local*clr-namespace:GenealogicalTreeCource.Class  5   	   _ [MaterialDesignThemes.Wpf, Version=5.1.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962a:md1http://materialdesigninxaml.net/winfx/xaml/themes  5	   	     � 
ResizeMode$  NoResize=�5   	    � Title$+ %Генеалогічне Дерево��5   	   $	��640��5   	   $
��1000��6   
������� � WindowStyle$
 None=�5   	    � 	Resources 5      -   'GenealogicalTreeCource.Class.PersonTree   MyPersonTree �� 
TargetType �� Property �� Value �� 
StartPoint �� EndPoint	 � Color
 � Offset3  -System.Windows.Media.Effects.DropShadowEffect   
BlurRadius   ShadowDepth   Opacity   Color �� 
TargetType �� CornerRadius �� Triggers P� Property P� Value�� 5   
   %�  &	        '��        5      6   �� 5      " ��6   .�5      ��  � ��$ 36(�6-   �� 5       � ��$ 80��6+   �� 5       � ��$	 300��6*   �� 5       � ��
 �����6/   �� 5       � ��$ 20q�6,   �� 5       � ��$ 0,0,0,20q�6+   �� 5       � �� 5      �� 5      $	 0,0&�6.   $	 1,0&�6?   .��5      � $	 #4CAF50��6+   $	
 0.0Q�6;   � 5      $	 #124213��6+   $	
 1.0Q�6;   5       5"      �� 5#       �@J�Effect 5$        5%      $ 15Q�6+   $ 5Q�6;   $	 0.4Q�6K   $ Black��6Y   5'      �� 5(       � �� 5)      �� 5*      " ��6*   .R�5+      � .+�5,   "   �� #��z��6)   $ 40��6S   ._�5-   &   �� $��Center=�67   $��Center=�6T   5.   #   5/      50      52       53      P� 54       �@J�IsMouseOver$
 True��65   . �55      ��  �@J�Opacity$	 0.8Q�64   56      58      59       � DataContext 5<      �� 5=   
    �� ResourceKey$ MyPersonTree��6   .��5@      � ��5A   
   �� 5B       �� ImageSource$+ %/Image/MainWindow/tree_background.jpg��6    j� Stretch$ UniformToFill=�6M   .+�5F   
   �� 
���   � J� Opacity$	 0.3Q�6!   $��Stretch=�6/   $��Stretch=�6M   �� 5H   
   $P��JРозроби своє власне генеалогічне дерево��6   $��36(�5I      $
��Bold"�6"   
������$��Comic Sans MS)�5J      $
��Left=�5K      $	��Top=�6/   $��
151,20,0,0q�5L      $
��Wrap=�6(    �� TextAlignment$
 Left=�5M       �� 
LineHeight$ 40��6)    J� Effect 5N        5O      $ 2Q�6#   $ 5Q�63   $ Black��6B   5Q      �� 5S   
   $��Center=�6   $��Center=�60   .+�5U      �� $��ТВОЄ ДЕРЕВО��6   ��6,   �� *	GraphPage+�� 5V      $��ДОДАТИ��6   ��6&   �� *AddPersonPage+5W      �� 5[   
   -   5        $��Right=�5[      $	��Top=�6-   $��
0,548,10,0q�6E   $	��163��5\   	   $��50��6   
������ ._�5^      �� -   5         ��Name$ AdministratorButton��5^      $��/Image/adminButton.png��61   $	��163��6Q   $��50��6]   $��Right=�5_      $	��Top=�6(   $y�Hidden=�6@    5`        5a      $ 10Q�6'   $ 3Q�67   $	 0.5Q�6G   $ Black��6U   ��5c      �� 5d      " ��6   .�5e      ��  �@J�Opacity$	 0.5Q�64    5f      P� 5g       �@J�IsMouseOver$
 True��6=   . �5h   "   ��  �@J�Opacity$ 1Q�6<   5i      5k      *  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5m      +  %Microsoft.Xaml.Behaviors.EventTrigger  5n         	EventName$ MouseLeftButtonDown��6%   *  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 5o      2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction      Command  60   �� *AdministrationPage+5p      5r      5s      �� 5u   
   $��0q�6   ._�5v       � -   5        !  �Name$! 	SetWindow��5v      "  � NavigationUIVisibility$" Hidden=�6'   5w      5x      5y      