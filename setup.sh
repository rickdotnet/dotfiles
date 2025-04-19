#!/bin/bash

DOTFILES_DIR=$(pwd)

mkdir -p ~/.config/bash

ln -sf "$DOTFILES_DIR/config/vim/.ideavimrc" ~/.ideavimrc || { echo "Failed to link .ideavimrc"; exit 1; }
ln -sf "$DOTFILES_DIR/config/bash/aliases" ~/.config/bash/aliases || { echo "Failed to link aliases"; exit 1; }

if ! grep -q "source ~/.config/bash/aliases" ~/.bashrc; then
    echo "source ~/.config/bash/aliases" >> ~/.bashrc
    echo "Added 'source ~/.config/bash/aliases' to ~/.bashrc"
fi

echo "Dotfiles setup complete! Run 'source ~/.bashrc' to apply changes in the current shell.\n"