@tool
extends EditorPlugin

func _enter_tree():
    add_custom_type("SKControl", "Control", preload("SKControl.cs"), null)

func _exit_tree():
    remove_custom_type("SKControl")
